using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Attendance;

public class AttendanceMappingProfile:Profile
{
    public AttendanceMappingProfile()
    {
        CreateMap<AttendanceRequest, Domain.Entities.Attendance>();
        CreateMap<Domain.Entities.Attendance, AttendanceResponse>();
    }
}