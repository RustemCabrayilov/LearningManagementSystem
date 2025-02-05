using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class DocumentsController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    // GET
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var response = await _learningManagementSystem.GetDocument(id);
            return View(response);
        }
        catch (ApiException e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
    

}