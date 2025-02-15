using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class VotesController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    // GET
    public async Task<IActionResult> Index(RequestFilter? filter)
    {
        var response = await _learningManagementSystem.VoteList(filter);
        int totalVotes = _learningManagementSystem.VoteList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalVotes / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
}