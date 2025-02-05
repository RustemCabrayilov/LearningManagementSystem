using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Controllers;

public class AttendancesController(ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    // GET
    public async  Task<IActionResult> CheckAttendance(Guid lessonId)
    {
        var attendance = await _learningManagementSystem.AttendanceList(new RequestFilter()
            { FilterField = "LessonId", FilterGuidValue = lessonId });

        return View(attendance.ToArray());
    }

    [HttpPost]
    public async Task<IActionResult> CheckAttendance(AttendanceResponse[] attendances)
    {
        try
        {
            List<AttendanceRequest> requests = new List<AttendanceRequest>();
            foreach (var attendance in attendances)
            {
                requests.Add(new AttendanceRequest(attendance.Id, attendance.Student.Id, attendance.Lesson.Id,
                    attendance.Absence));
            }

            var response = await _learningManagementSystem.UpdateAttendanceList(requests.ToArray());
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value[0].FirstOrDefault().ToString());
            return RedirectToAction("CheckAttendance");
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }
            return RedirectToAction("CheckAttendance");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("CheckAttendance");
        }

        return RedirectToAction("Index", "Exams");

    }
    
}