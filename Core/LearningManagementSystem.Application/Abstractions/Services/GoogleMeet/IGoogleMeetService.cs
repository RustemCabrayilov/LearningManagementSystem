namespace LearningManagementSystem.Application.Abstractions.Services.GoogleMeet;

public interface IGoogleMeetService
{
    ValueTask<string> GenerateMeet();
}