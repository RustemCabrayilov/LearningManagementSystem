using LearningManagementSystem.Application.Abstractions.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> Post(SignInRequest request)
    {
        var response = await _authService.SignInAsync(request);
        Console.WriteLine(User.Identity.Name);
        return Ok(response);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> Post(SignUpRequest request)
    {
        var response = await _authService.SignUpAsync(request); 
        return Ok(response);
    }
    [HttpGet("confirm-email")]
    public async Task<IActionResult> Post([FromQuery]string email,string token)
    {
        await _authService.ConfirmEmailAsync(email, token); 
        return Ok();
    }

}