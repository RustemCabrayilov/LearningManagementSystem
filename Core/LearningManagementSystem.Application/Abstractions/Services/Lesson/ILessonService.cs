namespace LearningManagementSystem.Application.Abstractions.Services.Lesson;

public interface ILessonService
{
    Task<LessonResponse> CreateAsync(LessonRequest dto);
    Task<LessonResponse> UpdateAsync(Guid id,LessonRequest dto);
    Task<LessonResponse> RemoveAsync(Guid id);
    Task<LessonResponse> GetAsync(Guid id);
    Task<IList<LessonResponse>> GetAllAsync();
}