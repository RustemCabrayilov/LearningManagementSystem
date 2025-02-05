using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.BackgroundJob;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Infrastructure.Services.BackgroundJob;

public class BackgroundJobService(
    IGenericRepository<Teacher> _teacherRepository,
    IGenericRepository<Student> _studentRepository,
    IGenericRepository<Exam> _examRepository,
    IGenericRepository<Transcript> _transcriptRepository) : IBackgroundJobService
{
    public async Task Recommendteacher()
    {
        var teachers = await _teacherRepository.GetAll(x =>  !x.IsDeleted, new ()
        {
            SortField = "Rate"
        }).ToListAsync();
        foreach (var teacher in teachers)
        {
            Console.WriteLine(teacher.Name+" "+teacher.Surname);
        }
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
}