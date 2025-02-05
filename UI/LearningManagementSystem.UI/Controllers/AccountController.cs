using LearningManagementSystem.Application.Abstractions.Services.Auth;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Controllers;

public class AccountController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.SignIn(request);
            HttpContext.Response.Cookies.Append("access_token", response.AccessToken, new CookieOptions()
            {
                HttpOnly = false,
                Path = "/",
                Expires = response.Expiration, Secure = false, SameSite = SameSiteMode.Lax
            });
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            Console.WriteLine(User);
            _toastNotification.AddSuccessToastMessage("User signed in");
            return RedirectToAction("Index", "Home");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return View();
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }
            return View();
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage("Something went wrong");
            return View();
        }
    }

    public IActionResult LogOut()
    {
        HttpContext.Response.Cookies.Delete("access_token");
        _toastNotification.AddErrorToastMessage("User logged out");
        return RedirectToAction("SignIn", "Account");
    }
}