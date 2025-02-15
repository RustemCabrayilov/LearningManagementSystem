﻿using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(RetakeExamConfiguration))]
public class RetakeExam:BaseEntity
{
    public Exam Exam { get; set; }
    public Guid ExamId { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime ApplyDate { get; set; }
    public RetakeExamType RetakeExamType { get; set; }
    public decimal Price { get; set; }
    public List<StudentRetakeExam> StudentRetakeExams { get; set; }

}