using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Theme;

public interface IThemeService
{
    Task<ThemeResponse> CreateAsync(ThemeRequest dto);
    Task<ThemeResponse> UpdateAsync(Guid id,ThemeRequest dto);
    Task<ThemeResponse> RemoveAsync(Guid id);
    Task<ThemeResponse> GetAsync(Guid id);
    Task<IList<ThemeResponse>> GetAllAsync(RequestFilter? filter);
    Task<ThemeResponse> ActivateAsync(Guid id);

}