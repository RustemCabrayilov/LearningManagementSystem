using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class TeachersController(ITeacherService _teacherService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromForm]TeacherRequest request)
    {
        var response = await _teacherService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _teacherService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _teacherService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, TeacherRequest request)
    {
        var response = await _teacherService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _teacherService.RemoveAsync(id); 
        return Ok(response);
    }
    [HttpPost("assign-group")]
    public async Task<IActionResult> Post(TeacherGroupDto request)
    {
        var response = await _teacherService.AssignGroupAsync(request); 
        return Ok(response);
    }
}