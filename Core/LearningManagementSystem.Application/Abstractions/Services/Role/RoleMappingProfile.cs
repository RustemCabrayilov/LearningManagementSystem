using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Role;

public class RoleMappingProfile:Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Domain.Entities.Identity.AppRole, RoleResponse>();
        CreateMap<RoleRequest,Domain.Entities.Identity.AppRole>();
    }   
}