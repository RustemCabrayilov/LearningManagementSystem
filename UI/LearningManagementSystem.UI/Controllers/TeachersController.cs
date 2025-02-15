using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class TeachersController(ILearningManagementSystem _learningManagementSystem ) : Controller
{
    public async Task<IActionResult> Details(string userId)
    {
        var teachers=await _learningManagementSystem.TeacherList(new RequestFilter{FilterField = "AppUserId",FilterValue = userId});
        var teacher=teachers.FirstOrDefault();
        ViewBag.User=await _learningManagementSystem.GetUser(userId);
        return View(teacher);
    }
    public async Task<IActionResult> TeacherSurveys(RequestFilter? filter)
    {
        var responses = await _learningManagementSystem.SurveyList(filter);
        int totalVotes = _learningManagementSystem.VoteList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalVotes / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        // Return the schedule data in a format that can be easily consumed by JavaScript
        return View(responses);
    }
}