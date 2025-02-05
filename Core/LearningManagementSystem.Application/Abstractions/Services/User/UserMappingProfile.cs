using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.User;

public class UserMappingProfile:Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserRequest, Domain.Entities.Identity.AppUser>();
        CreateMap<Domain.Entities.Identity.AppUser, UserResponse>();
        CreateMap<UserRoleDto,Domain.Entities.Identity.AppUser>();
    }   
}