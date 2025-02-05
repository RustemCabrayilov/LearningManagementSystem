using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;

public class GroupScheduleMappingProfile:Profile
{
    public GroupScheduleMappingProfile()
    {
        CreateMap<GroupScheduleRequest, Domain.Entities.GroupSchedule>();
        CreateMap<Domain.Entities.GroupSchedule,GroupScheduleResponse>();
    }
}