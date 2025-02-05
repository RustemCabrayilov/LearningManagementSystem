using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using NuGet.Packaging;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("admin")]
public class UsersController(ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var responses = await _learningManagementSystem.UserList(filter);
        int totalUsers = _learningManagementSystem.UserList(new RequestFilter() { AllUsers = true }).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
     
        return View(responses);
    }

    public  IActionResult Edit(string id)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] string id, UserRequest request)
    {
        var response = await _learningManagementSystem.UpdateUser(id, request);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _learningManagementSystem.RemoveUser(id);
        return RedirectToAction("Index");
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateUser(request);
            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return RedirectToAction("Create");
        }
        catch (ApiException e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("Create");
        }
    }

    public async Task<IActionResult> AssignRole(string id)
    {
        var roles = await _learningManagementSystem.RoleList(null);
        var model = new UserRoleDto(id, string.Empty, roles);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(UserRoleDto request)
    {
        var response = await _learningManagementSystem.AssignRole(request);
        return RedirectToAction("Index");
    }

    /*public async Task<IActionResult> Details([FromQuery]string userId)
    {
        var deans = await _learningManagementSystem.DeanList(new RequestFilter
            { AllUsers = true, FilterField = "AppUserId", FilterValue = userId });
        var teachers = await _learningManagementSystem.TeacherList(new RequestFilter
            { AllUsers = true, FilterField = "AppUserId", FilterValue = userId });
        var students = await _learningManagementSystem.StudentList(new RequestFilter
            { AllUsers = true, FilterField = "AppUserId", FilterValue = userId });
        object response;
        if (deans.Count > 0)
        {
            return RedirectToAction("DetailsByUser","Deans", new { userId=userId });

        }
        if (teachers.Count > 0)
        {
            return RedirectToAction("DetailsByUser","Teachers", new { userId=userId });
        }

        if (students.Count > 0)
        {
            return RedirectToAction("DetailsByUser","Students", new { userId=userId });

        }
        var student = await _learningManagementSystem.GetUser(userId);
        return View(student);
    }*/
}