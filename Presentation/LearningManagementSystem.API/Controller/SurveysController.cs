using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class SurveysController(ISurveyService _surveyService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(SurveyRequest request)
    {
        var response = await _surveyService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _surveyService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _surveyService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, SurveyRequest request)
    {
        var response = await _surveyService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _surveyService.RemoveAsync(id); 
        return Ok(response);
    }
}