using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class RetakeExamsController(IRetakeExamService _retakeExamService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(RetakeExamRequest request)
    {
        var response = await _retakeExamService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _retakeExamService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _retakeExamService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, RetakeExamRequest request)
    {
        var response = await _retakeExamService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _retakeExamService.RemoveAsync(id); 
        return Ok(response);
    }
}