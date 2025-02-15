using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.BackgroundJob;
using LearningManagementSystem.Application.Abstractions.Services.Email;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Infrastructure.Services.BackgroundJob;

public class BackgroundJobService(
    IGenericRepository<Teacher> _teacherRepository,
    IGenericRepository<Student> _studentRepository,
    IGenericRepository<StudentGroup> _studentGroupRepository,
    IGenericRepository<Attendance> _attendanceRepository,
    IGenericRepository<Transcript> _transcriptRepository,
    IGenericRepository<Term> _termRepository,
    IGenericRepository<Subject> _subjectRepository,
    IGenericRepository<Lesson> _lessonRepository,
    IGenericRepository<Group> _groupRepository,
    IGenericRepository<StudentSubject> _studentSubjectRepository,
    IEmailService _emailService,
    IUnitOfWork _unitOfWork) : IBackgroundJobService
{
    public async Task Recommendteacher()
    {
        var teachers = await _teacherRepository.GetAll(x => !x.IsDeleted, new()
        {
            SortField = "Rate"
        }).OrderBy(x=>x.Rate).ToListAsync();
        foreach (var teacher in teachers)
        {
            Console.WriteLine(teacher.Name + " " + teacher.Surname);
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

    public async Task FailNotification()
    {
        /*var students=await _studentRepository.GetAll(x => !x.IsDeleted, new RequestFilter() { AllUsers = true }).ToListAsync();*/
        var term = await _termRepository.GetAsync(x => !x.IsDeleted && x.IsActive);
        var activeTermGroups = await _groupRepository.GetAll(x => !x.IsDeleted && x.TermId == term.Id, new()
        {
            AllUsers = true
        }).ToListAsync();
        foreach (var activeTermGroup in activeTermGroups)
        {
            var studentGroups = await _studentGroupRepository.GetAll(
                x => x.GroupId == activeTermGroup.Id && !x.IsDeleted, new()
                {
                    AllUsers = true
                }).ToListAsync();
            foreach (var studentGroup in studentGroups)
            {
                var lessons = await _lessonRepository.GetAll(x => x.GroupId == studentGroup.GroupId && !x.IsDeleted,
                    new()
                    {
                        AllUsers = true
                    }).ToListAsync();
                List<Attendance> absences = new();
                foreach (var lesson in lessons)
                {
                    var attendance = await _attendanceRepository.GetAsync(x => !x.IsDeleted
                                                                               && x.StudentId == studentGroup.StudentId
                                                                               && x.LessonId == lesson.Id && x.Absence);
                    absences.Add(attendance);
                }

                var subject =
                    await _subjectRepository.GetAsync(x => x.Id == activeTermGroup.SubjectId && !x.IsDeleted);
                if (absences.Count() >= subject.AttendanceLimit)
                {
                    var studentSubject = await _studentSubjectRepository.GetAsync(x =>
                        !x.IsDeleted && x.StudentId == studentGroup.StudentId && x.SubjectId == subject.Id);
                    if (studentSubject.SubjectTakingType == SubjectTakingType.Taking)
                    {
                        studentSubject.SubjectTakingType = SubjectTakingType.Failed;
                        _studentSubjectRepository.Update(studentSubject);
                        _unitOfWork.SaveChanges();
                        var student =
                            await _studentRepository.GetAsync(x => x.Id == studentGroup.StudentId && !x.IsDeleted);
                        _studentGroupRepository.Update(studentGroup);
                        _unitOfWork.SaveChanges();
                      await  _emailService.SendEmailAsync(student?.AppUser?.Email,"Fail Notification",$"{student?.Name} you failed for attendance missing");
                        Console.WriteLine($"{student.Name} you failed for attendance missing");
                    }
                }
            }
        }
    }
}