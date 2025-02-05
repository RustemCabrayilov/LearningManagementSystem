using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Theme;

public class ThemeMappingProfile:Profile
{
    public ThemeMappingProfile()
    {
        CreateMap<ThemeRequest, Domain.Entities.Theme>();
        CreateMap<Domain.Entities.Theme, ThemeResponse>();
    }
}