using LearningManagementSystem.Application.Abstractions.Services.GoogleMeet;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]

public class GoogleMeetsController(IGoogleMeetService _googleMeetService) : ControllerBase
{
    // GET
    [HttpGet]
    public async Task<IActionResult> GenerateMeet()
    {
        var response=await _googleMeetService.GenerateMeet();
        return Ok(response);
    }
}