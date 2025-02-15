using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Controllers;

public class StudentsController(
    ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> StudentGroups()
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var students = await _learningManagementSystem.StudentList(new RequestFilter()
            { FilterField = "AppUserId", FilterValue = userclaim.Id });
        var student = students.FirstOrDefault();
        var response = await _learningManagementSystem.GetStudent(student.Id);
        ViewBag.Groups = response.Groups;
        return View();
    }
    public async Task<IActionResult> Details(string userId)
    {
        var students = await _learningManagementSystem.StudentList(new RequestFilter
            { FilterField = "AppUserId", FilterValue = userId });
        var student = students.FirstOrDefault();
        ViewBag.User = await _learningManagementSystem.GetUser(userId);
        var qrCodeStream = await _learningManagementSystem.GenerateQRCode(student.Id);

        using var memoryStream = new MemoryStream();
        await qrCodeStream.CopyToAsync(memoryStream);
        var qrCodeBytes = memoryStream.ToArray();
        var base64String = Convert.ToBase64String(qrCodeBytes);
        string qrCodeUrl = $"data:image/png;base64,{base64String}";
        student = student with { QrCodeUrl = qrCodeUrl };
        return View(student);
    }

    public async Task<IActionResult> AssignGroup()
    {
        ViewBag.Groups = await _learningManagementSystem.GroupList(new RequestFilter
            { FilterField = "CanApply", FilterValue = "true" });
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AssignGroup([FromBody] StudentGroupDto[] studentGroupDtos)
    {
        try
        {
            var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
            var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
            var students = await _learningManagementSystem.StudentList(new RequestFilter()
            {
                FilterField = "AppUserId", FilterValue = userclaim.Id,
                AllUsers = true
            });
            var student = students.FirstOrDefault();
            List<StudentGroupDto> requests = new();
            foreach (var studentGroupDto in studentGroupDtos)
            {
                requests.Add(studentGroupDto with { StudentId = student.Id });
            }

            var responses = await _learningManagementSystem.AssignGroupList(requests.ToArray());
            return RedirectToAction("Index", "Home");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return RedirectToAction("AssignGroup");
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }

            return RedirectToAction("AssignGroup");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("AssignGroup");
        }
    }

    public async Task<IActionResult> AssignPointAsync([FromQuery] Guid examId)
    {
        var studentExams = await _learningManagementSystem.StudentExamList(new RequestFilter()
            { FilterField = "ExamId", FilterGuidValue = examId });

        return View(studentExams);
    }

    [HttpPost]
    public async Task<IActionResult> AssignPointAsync(StudentExamResponse[] studentExams)
    {
        try
        {
            List<StudentExamRequest> requests = new List<StudentExamRequest>();
            foreach (var studentExam in studentExams)
            {
                requests.Add(new StudentExamRequest(studentExam.Id, studentExam.Exam.Id, studentExam.Student.Id,
                    studentExam.Point));
            }

            var response = await _learningManagementSystem.UpdateStudentExam(requests.ToArray());
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value[0].FirstOrDefault()
                .ToString());
            return RedirectToAction("AssignPoint", new { examId = studentExams[0].Exam.Id });
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }

            return RedirectToAction("AssignPoint", new { examId = studentExams[0].Exam.Id });
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("AssignPoint", new { examId = studentExams[0].Exam.Id });
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> AssignAttendanceAsync([FromQuery] Guid lessonId)
    {
        var attendances = await _learningManagementSystem.AttendanceList(new RequestFilter()
        {
            AllUsers = true,
            FilterField = "LessonId", FilterGuidValue = lessonId
        });

        return View(attendances.ToArray());
    }

    [HttpPost]
    public async Task<IActionResult> AssignAttendanceAsync(AttendanceResponse[] attendances, Guid lessonId)
    {
        try
        {
            List<AttendanceRequest> requests = new List<AttendanceRequest>();
            foreach (var attendance in attendances)
            {
                requests.Add(new AttendanceRequest(attendance.Id, attendance.Student.Id, lessonId,
                    attendance.Absence));
            }

            var response = await _learningManagementSystem.UpdateAttendanceList(requests.ToArray());
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return RedirectToAction("AssignPoint");
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }

            return RedirectToAction("AssignAttendance");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("AssignAttendance");
        }

        return RedirectToAction("Index", "Lessons");
    }

    public async Task<IActionResult> AssignRetakeExam(Guid id)
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var students = await _learningManagementSystem.StudentList(new RequestFilter()
            { FilterField = "AppUserId", FilterValue = userclaim.Id });
        var student = students.FirstOrDefault();
        try
        {
            var response = await _learningManagementSystem.AssignRetakeExam(new StudentRetakeExamDto(
                student.Id, id, Status.Pending, 0
            ));
            return RedirectToAction("Index", "Home");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return RedirectToAction("AssignPoint");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
        }

        return RedirectToAction("Index", "RetakeExams");
    }

    public async Task<IActionResult> CheckStudentId([FromQuery] Guid id)
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var students = await _learningManagementSystem.StudentList(new RequestFilter()
            { FilterField = "AppUserId", FilterValue = userclaim.Id });
        var student = students.FirstOrDefault();
        return Json(new
        {
            result = student?.Id == id,
        });
    }
}