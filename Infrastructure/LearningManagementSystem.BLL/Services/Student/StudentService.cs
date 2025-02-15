using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Student;

public class StudentService(
    IGenericRepository<Domain.Entities.Student> _studentRepository,
    IGenericRepository<Domain.Entities.StudentGroup> _studentGroupRepository,
    UserManager<AppUser> _userManager,
    IGenericRepository<StudentSubject> _studentSubjectRepository,
    IStudentExamService _studentExamService,
    IGenericRepository<Domain.Entities.StudentExam> _studentExamRepository,
    IGenericRepository<Domain.Entities.StudentRetakeExam> _studentRetakeExamRepository,
    IGenericRepository<Domain.Entities.Subject> _subjectRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IGenericRepository<Domain.Entities.Term> _termRepository,
    IGenericRepository<Domain.Entities.Exam> _examRepository,
    IGenericRepository<Domain.Entities.RetakeExam> _retakeExamRepository,
    IGenericRepository<Domain.Entities.Vote> _voteRepository,
    IRedisCachingService _redisCachingService,
    IDocumentService _documentService,
    ITranscriptService _transcriptService,
    IGenericRepository<Domain.Entities.Transcript> _transcriptRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IStudentService
{
    public async Task<StudentResponse> CreateAsync(StudentRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Student>(dto);
        await _studentRepository.AddAsync(entity);
        await _documentService.CreateByOwnerAsync(new DocumentByOwner(new() { dto.File }, entity.Id,
            DocumentType.Student));
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);
    }

    public async Task<StudentResponse> UpdateAsync(Guid id, StudentRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<StudentResponse>(key);
        var entity = await _studentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student not found");
        _mapper.Map(dto, entity);
        _studentRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var document = await _documentService.GetByOwnerId(id);
        await _documentService.UpdateAsync(document.Id, new(document.Id, document.DocumentType, document.Path,
            document.Key,
            document.FileName, document.OriginName, document.OwnerId, new() { dto.File }));
        var outDto = _mapper.Map<StudentResponse>(entity);
        if (data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<StudentResponse> RemoveAsync(Guid id)
    {
        var entity = await _studentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student not found");
        _studentRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<StudentResponse>(entity);
    }

    public async Task<StudentResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<StudentResponse>(key);
        if (data is not null)
        {
            return data;
        }

        var entity = await _studentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student not found");
        var user = await _userManager.FindByIdAsync(entity?.AppUserId);
        var term = await _termRepository.GetAsync(x => !x.IsDeleted && x.IsActive == true);
        var studentGroups = await _studentGroupRepository.GetAll(x => !x.IsDeleted && x.StudentId == entity.Id,
            new RequestFilter()
            {
                AllUsers = true
            }).ToListAsync();
        var groupList = new List<GroupResponse>();
        foreach (var studentGroup in studentGroups)
        {
            var group = await _groupRepository.GetAsync(x =>
                !x.IsDeleted && x.Id == studentGroup.GroupId && x.TermId == term.Id);
            groupList.Add(_mapper.Map<GroupResponse>(group));
        }

        var studentExams = await _studentExamService.GetAllAsync(
            new RequestFilter()
            {
                AllUsers = true,
                FilterGuidValue = entity.Id,
                FilterField = "StudentId"
            });

        var studentRetakeExams = await _studentRetakeExamRepository.GetAll(
            x => !x.IsDeleted && x.StudentId == entity.Id,
            new RequestFilter()
            {
                AllUsers = true,
            }).ToListAsync();
        var studentExamList = new List<StudentExamResponse>();
        foreach (var studentExam in studentExams)
        {
            var exam = await _examRepository.GetAsync(x => x.Id == studentExam.Exam.Id && !x.IsDeleted);
            var group = await _groupRepository.GetAsync(x => x.Id == exam.GroupId && x.TermId == term.Id);
            if (group is not null)
            {
                studentExamList.Add(studentExam);
            }
        }

        var studentRetakeExamList = new List<Domain.Entities.StudentRetakeExam>();
        foreach (var studentRetakeExam in studentRetakeExams)
        {
            var retakeExam =
                await _retakeExamRepository.GetAsync(x => x.Id == studentRetakeExam.RetakeExamId && !x.IsDeleted);
            var exam = await _examRepository.GetAsync(x => x.Id == retakeExam.ExamId && !x.IsDeleted);
            var group = await _groupRepository.GetAsync(x => x.Id == exam.GroupId && x.TermId == term.Id);
            if (group is not null)
            {
                studentRetakeExamList.Add(studentRetakeExam);
            }
        }

        var transcripts = await _transcriptRepository.GetAll(x => !x.IsDeleted, new()
        {
            AllUsers = true,
            FilterGuidValue = entity.Id,
            FilterField = "StudentId"
        }).Include(x => x.Group).ToListAsync();
        var outDto = _mapper.Map<StudentResponse>(entity) with
        {
            AppUser = _mapper.Map<UserResponse>(user),
            Groups = groupList,
            StudentExams = studentExams,
            StudentRetakeExams = _mapper.Map<IList<StudentRetakeExamResponse>>(studentRetakeExamList),
            Transcripts = _mapper.Map<IList<TranscriptResponse>>(transcripts)
        };
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<StudentResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _studentRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<StudentResponse>>(entities);
    }

    public async Task<StudentResponse> AssignGroupAsync(StudentGroupDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentGroup>(dto);
        await _studentGroupRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);
    }

    public async Task<StudentGroupDto[]> AssignGroupsAsync(StudentGroupDto[] dtos)
    {
        var groupIds = dtos.Select(x => x.GroupId).ToArray();
        var groups = new List<Domain.Entities.Group>();
        var subjectIds = _subjectRepository.GetAll(x => !x.IsDeleted, new RequestFilter() { AllUsers = true })
            .Select(x => x.Id).ToArray();
        foreach (var groupId in groupIds)
        {
            var group = await _groupRepository.GetAsync(x => x.Id == groupId && !x.IsDeleted);
            groups.Add(group);
        }

        foreach (var subjectId in subjectIds)
        {
            int count = 0;
            foreach (var group in groups)
            {
                if (group.SubjectId == subjectId)
                {
                    count++;
                }
            }

            if (count >= 2)
            {
                throw new BadRequestException("Two or more same subject cannot be took");
            }
        }

        var entities = _mapper.Map<Domain.Entities.StudentGroup[]>(dtos);
        var term = await _termRepository.GetAsync(x => !x.IsDeleted && x.IsActive);
        var activeTermGroups = await _groupRepository.GetAll(x => !x.IsDeleted && x.TermId == term.Id, new()
        {
            AllUsers = true
        }).ToListAsync();
        foreach (var activeTermGroup in activeTermGroups)
        {
            var studentGroup = await _studentGroupRepository.GetAsync(x =>
                !x.IsDeleted && x.StudentId == dtos[0].StudentId && x.GroupId == activeTermGroup.Id);
            if (studentGroup is not null)
            {
                _studentGroupRepository.Remove(studentGroup);
                _unitOfWork.SaveChanges();
            }
        }

        foreach (var entity in entities)
        {
            var studentGroup = await _studentGroupRepository.GetAsync(x =>
                x.StudentId == entity.StudentId && x.GroupId == entity.GroupId && !x.IsDeleted);
            if (studentGroup is not null)
            {
                _studentGroupRepository.Update(studentGroup);
                _unitOfWork.SaveChanges();
            }
            else
            {
                await _studentGroupRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        foreach (var entity in entities)
        {
            await _transcriptService.CreateAsync(new(entity.StudentId, entity.GroupId, 0));
        }

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentGroupDto[]>(entities);
    }

    public async Task<StudentResponse> AssignSubjectAsync(StudentSubjectDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentSubject>(dto);
        await _studentSubjectRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);
    }

    public async Task<StudentResponse> AssignExamAsync(StudentExamDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentExam>(dto);
        await _studentExamRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);
    }

    public async Task<StudentRetakeExamResponse> AssignRetakeExamAsync(StudentRetakeExamDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentRetakeExam>(dto);
        await _studentRetakeExamRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentRetakeExamResponse>(entity);
    }
}