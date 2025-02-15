using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Persistence.Context;

public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Transcript> Transcripts { get; set; }
    public DbSet<Dean> Deans { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Major> Majors { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Term> Terms { get; set; }
    public DbSet<RetakeExam> RetakeExams { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<StudentExam> StudentExams { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }
    public DbSet<StudentRetakeExam> StudentRetakeExams { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<SubjectMajor> SubjectMajors { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Chat> Chats { get; set; }
}