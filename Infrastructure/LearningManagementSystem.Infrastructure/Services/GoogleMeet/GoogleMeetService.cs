using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using LearningManagementSystem.Application.Abstractions.Services.GoogleMeet;

namespace LearningManagementSystem.Infrastructure.Services.GoogleMeet;

public class GoogleMeetService: IGoogleMeetService
{
    public async ValueTask<string> GenerateMeet()
    {
        var credential = GoogleCredential.FromFile("C:\\Users\\ASUS\\Desktop\\FinalProject\\LearningManagementSystem\\Infrastructure\\LearningManagementSystem.Infrastructure\\Credentials\\unified-ion-447716-n4-41c2d567bbe6.json")
            .CreateScoped(CalendarService.Scope.Calendar);

        var service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "LearningManagementSystem",
        });
        var eventItem = new Event
        {
            Summary = "Important Meeting",
            Start = new EventDateTime { DateTime = DateTime.Now.AddHours(0) },
            End = new EventDateTime { DateTime = DateTime.Now.AddHours(2) },
            ConferenceData = new ConferenceData
            {
                CreateRequest = new CreateConferenceRequest { RequestId = Guid.NewGuid().ToString() }
            }
        };

        var request = service.Events.Insert(eventItem, "primary");
        request.ConferenceDataVersion = 1;
        var createdEvent = request.Execute();

        Console.WriteLine($"Meet link: {createdEvent.HangoutLink}");
        return createdEvent.HangoutLink;
    }
}
