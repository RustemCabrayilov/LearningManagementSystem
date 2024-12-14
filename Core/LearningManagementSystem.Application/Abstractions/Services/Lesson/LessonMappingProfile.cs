using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Lesson;

public class LessonMappingProfile:Profile
{
    public LessonMappingProfile()
    {
        CreateMap<LessonRequest, Domain.Entities.Lesson>();
        CreateMap<Domain.Entities.Lesson, LessonResponse>();
    }
}