using AutoMapper;
using LearningManagementSystem.Domain.Entities.Identity;

namespace LearningManagementSystem.Application.Abstractions.Services.Auth;

public class AuthMappingProfile:Profile
{
    public AuthMappingProfile()
    {
        CreateMap<SignUpRequest, AppUser>();
    }
}