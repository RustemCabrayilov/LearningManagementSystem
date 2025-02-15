using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class SurveysController(ISurveyService _surveyService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Post(SurveyRequest request)
    {
        var response = await _surveyService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _surveyService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _surveyService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<SurveyRequest>))]
    public async Task<IActionResult> Put(Guid id, SurveyRequest request)
    {
        var response = await _surveyService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _surveyService.RemoveAsync(id); 
        return Ok(response);
    }
    [HttpPost("activate")]
    public async Task<IActionResult> Post(Guid id)
    {
        var response = await _surveyService.Activate(id);
        return Ok(response);
    } 
}