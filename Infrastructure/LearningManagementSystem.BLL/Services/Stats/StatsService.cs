using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Stats;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Stats;

public class StatsService(
    IGenericRepository<Domain.Entities.Teacher> _teacherRepository,
    IGenericRepository<Domain.Entities.Student> _studentRepository,
    IGenericRepository<Domain.Entities.Transcript> _transcriptRepository,
    IGenericRepository<StudentSubject> _studentSubjectRepository,
    IGenericRepository<Domain.Entities.Subject> _subjectRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IMapper _mapper) : IStatsService
{
    public async ValueTask<float> AverageTeacherRate()
    {
        var teachers = await _teacherRepository.GetAll(x => !x.IsDeleted, new()
        {
            AllUsers = true
        }).ToListAsync();
        var averageRate = teachers.Average(x => x.Rate);
        return averageRate;
    }

    public async Task AveragOfStudent()
    {
        var students = await _studentRepository.GetAll(x => !x.IsDeleted, new RequestFilter() { AllUsers = true })
            .ToListAsync();
        foreach (var student in students)
        {
            var points = await _transcriptRepository
                .GetAll(x => x.StudentId == student.Id
                             && !x.IsDeleted,
                    new RequestFilter() { AllUsers = true }).Select(x => x.TotalPoint)
                .ToListAsync();
            var average = points.Average();
            Console.WriteLine(average);
        }
    }

    public async Task<List<StatsResponse<SubjectResponse>>> MostFailedSubjects()
    {
        var subjects = await _subjectRepository.GetAll(x => x.IsDeleted, new()
        {
            AllUsers = true
        }).ToListAsync();
        var mostFailedSubjects = new List<StatsResponse<SubjectResponse>>();

        foreach (var subject in subjects)
        {
            var subjectList = _studentSubjectRepository.GetAll(
                x => !x.IsDeleted && x.SubjectId == subject.Id && x.SubjectTakingType == SubjectTakingType.Failed, new()
                {
                    AllUsers = true
                });
          
                mostFailedSubjects.Add(new()
                {
                    Entity = _mapper.Map<SubjectResponse>(subject),
                    Count = subjectList.Count()
                });
        }

        return mostFailedSubjects.OrderBy(x => x.Count).Take(5).ToList();
    }

    public async Task<List<StatsResponse<TeacherResponse>>> MostEfficientTeachers()
    {
        var teachers = await _teacherRepository.GetAll(x => !x.IsDeleted,
            new()
            {
                AllUsers = true
            }).ToListAsync();
        
        var mostEfficientTeachers = new List<StatsResponse<TeacherResponse>>();

        foreach (var teacher in teachers)
        {
            var groups = _groupRepository.GetAll(
                x => !x.IsDeleted && x.SubjectId == teacher.Id, new()
                {
                    AllUsers = true
                });
                mostEfficientTeachers.Add(new()
                {
                    Entity = _mapper.Map<TeacherResponse>(teacher),
                    Count = groups.Count()
                });
            
        }

        return mostEfficientTeachers.OrderBy(x => x.Count).Take(5).ToList();
    }
}