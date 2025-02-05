using LearningManagementSystem.Application.Abstractions.Services.Role;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

    [Area("admin")]
public class RolesController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.RoleList(filter);
        int totalRoles = _learningManagementSystem.RoleList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalRoles / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(string id)
    {
        var model = await _learningManagementSystem.GetRole(id);
        ViewBag.TermSeasonList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(TermSeason)))
        {
            ViewBag.TermSeasonList.Add(term);
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(string id,RoleRequest request)
    {
        var response = await _learningManagementSystem.UpdateRole(id,request);
        return RedirectToAction("Index");
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(RoleRequest request)
    {
        var response = await _learningManagementSystem.CreateRole(request);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        await _learningManagementSystem.RemoveRole(id);
        return RedirectToAction("Index");
    }
}