using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(StudentConfiguration))]

public class Student:Person
{
    public string StudentNo { get; set; }
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public List<StudentExam> StudentExams { get; set; }
    public List<Attendance> Attendances { get; set; }
    public List<Vote> Votes { get; set; }
    public List<Transcript> Transcripts { get; set; }
    public List<StudentRetakeExam> StudentRetakeExams { get; set; }
    public List<StudentGroup> StudentGroups { get; set; }
    public List<StudentSubject> StudentSubjects { get; set; }

}