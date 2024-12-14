using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class SubjectsController (ISubjectService _subjectService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(SubjectRequest request)
    {
        var response = await _subjectService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _subjectService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _subjectService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, SubjectRequest request)
    {
        var response = await _subjectService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _subjectService.RemoveAsync(id); 
        return Ok(response);
    }
}