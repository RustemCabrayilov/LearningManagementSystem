using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Student;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Student;

public class StudentService(
    IGenericRepository<Domain.Entities.Student> _studentRepository,
    IGenericRepository<Domain.Entities.StudentGroup> _studentGroupRepository,
    IGenericRepository<Domain.Entities.StudentLesson> _studentLessonRepository,
    IGenericRepository<Domain.Entities.StudentMajor> _studentMajorRepository,
    IGenericRepository<Domain.Entities.StudentSubject> _studentSubjectRepository,
    IGenericRepository<Domain.Entities.StudentExam> _studentExamRepository,
    IGenericRepository<Domain.Entities.StudentRetakeExam> _studentRetakeExamRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IStudentService
{
    public async Task<StudentResponse> CreateAsync(StudentRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Student>(dto);
        await _studentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);
    }

    public async Task<StudentResponse> UpdateAsync(Guid id, StudentRequest dto)
    {
        var entity = await _studentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student not found");
        await _studentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);
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
        var entity = await _studentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student not found");
        return _mapper.Map<StudentResponse>(entity);
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

    public async Task<StudentResponse> AssignLessonAsync(StudentLessonDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentLesson>(dto);
        await _studentLessonRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);   
    }

    public async Task<StudentResponse> AssignMajorAsync(StudentMajorDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentMajor>(dto);
        await _studentMajorRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity); 
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

    public async Task<StudentResponse> AssignRetakeExamAsync(StudentRetakeExamDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentRetakeExam>(dto);
        await _studentRetakeExamRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentResponse>(entity);
    }
}