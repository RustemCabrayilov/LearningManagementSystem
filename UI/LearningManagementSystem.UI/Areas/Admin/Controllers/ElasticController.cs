using LearningManagementSystem.Application.Helper;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("admin")]
public class ElasticController(ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    // GET
    public async Task<IActionResult> Search(string value)
    {
        try
        {
            var response = await _learningManagementSystem.ElasticList(ElasticSearchHelper.SearchField, value);
            return Json(
                new
                {
                    results = response
                });
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddWarningToastMessage(errorMessage);
            }
            return Json(new
            {
                errorMessage="No result found"
            });
        }
        catch (Exception e)
        {
            return Json(new
            {
                errorMessage="No result found"
            });
        }
    }
}