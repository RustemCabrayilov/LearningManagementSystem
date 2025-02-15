using LearningManagementSystem.Application.Abstractions.Services.Stats;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class StatsController(IStatsService _statsService) : ControllerBase
{
    [HttpGet("most-failed-subjects")]
    public async Task<IActionResult> Get()
    {
        var response = await _statsService.MostFailedSubjects();
        return Ok(response);
    }

    [HttpGet("average-students")]
    public async Task<IActionResult> AvrgStudents()
    {
        var response = await _statsService.AveragOfStudent();
        return Ok(response);
    }

    [HttpGet("top-teacher-rate")]
    public async Task<IActionResult> AvrgRate()
    {
        var response = await _statsService.TopTeacherRate();
        return Ok(response);
    }
    [HttpGet("most-efficient-teachers")]
    public async Task<IActionResult> MostEfficientteacher()
    {
        var response = await _statsService.MostEfficientTeachers();
        return Ok(response);
    }
}