using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.OCR;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Teacher;

public class TeacherService(
    IGenericRepository<Domain.Entities.Teacher> _teacherRepository,
    IGenericRepository<Domain.Entities.Faculty> _facultyRepository,
    IGenericRepository<Domain.Entities.Vote> _voteRepository,
    IGenericRepository<Domain.Entities.Survey> _surveyRepository,
    IRedisCachingService _redisCachingService,
    UserManager<AppUser> _userManager,
    IDocumentService _documentService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : ITeacherService
{
    public async Task<TeacherResponse> CreateAsync(TeacherRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Teacher>(dto);
        entity.FacultyId=Guid.Parse(dto.FacultyId);
        await _teacherRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        await  _documentService.CreateByOwnerAsync(new DocumentByOwner(new(){dto.File},entity.Id,DocumentType.Teacher));
        return _mapper.Map<TeacherResponse>(entity);
    }

    public async Task<TeacherResponse> UpdateAsync(Guid id, TeacherRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<TeacherResponse>(key);
        var entity = await _teacherRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Teacher not found");
        _mapper.Map(dto, entity);
         _teacherRepository.Update(entity);
         _unitOfWork.SaveChanges();
         var document = await _documentService.GetByOwnerId(id);
         await _documentService.UpdateAsync(document.Id, new(document.Id,document.DocumentType,document.Path,document.Key,
             document.FileName,document.OriginName,document.OwnerId,new(){dto.File}));
         var outDto=_mapper.Map<TeacherResponse>(entity);
         if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<TeacherResponse> RemoveAsync(Guid id)
    {
        var entity = await _teacherRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Teacher not found");
        _teacherRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<TeacherResponse>(entity);
    }

    public async Task<TeacherResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<TeacherResponse>(key);
        if (data is not null)
            return data;
        var entity = await _teacherRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Teacher not found");
        entity.Faculty=await _facultyRepository.GetAsync(x=>x.Id == entity.FacultyId && !x.IsDeleted);
        entity.AppUser=await _userManager.FindByIdAsync(entity.AppUserId);
        var surveys = await _surveyRepository.GetAll(x => x.TeacherId == entity.Id, new()
        {
            AllUsers = true
        }).ToListAsync();
        var outDto = _mapper.Map<TeacherResponse>(entity) with
        {
            Surveys = _mapper.Map<IList<SurveyResponse>>(surveys)
        };
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<TeacherResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _teacherRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<TeacherResponse>>(entities);
    }
    
}