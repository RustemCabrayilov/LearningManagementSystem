using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace LearningManagementSystem.UI.Controllers;

public class SurveysController(ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var response = await _learningManagementSystem.SurveyList(new RequestFilter()
        {
            AllUsers = true
        });
        return View(response);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetSurvey(id);
        ViewBag.Questions = response.Questions;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Details(Guid id,[FromForm]VoteRequest[] votes)
    {
        try
        {
            var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
            var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
            var students = await _learningManagementSystem.StudentList(new RequestFilter()
                { FilterField = "AppUserId", FilterValue = userclaim.Id });
            var student = students.FirstOrDefault();
            List<VoteRequest> requests = new List<VoteRequest>();
            
            foreach (var vote in votes)
            {
                requests.Add(new VoteRequest(vote.Point, vote.QuestionId,student.Id,id));
            }
            var response = await _learningManagementSystem.CreateVoteList(requests.ToArray());

            return RedirectToAction("Index", "Home");
        }
        catch (ValidationApiException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (ApiException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}