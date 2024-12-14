using AutoMapper;
using LearningManagementSystem.Domain.Entities;

namespace LearningManagementSystem.Application.Abstractions.Services.Group;

public class GroupMappingProfile:Profile
{
    public GroupMappingProfile()
    {
        CreateMap<GroupRequest, Domain.Entities.Group>();
        CreateMap<Domain.Entities.Group, GroupResponse>();
        CreateMap<ExamGroupDto,ExamGroup>();
        CreateMap<ExamGroup,GroupResponse>();
    }
}