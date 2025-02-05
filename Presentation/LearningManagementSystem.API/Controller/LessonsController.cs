using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class LessonsController(ILessonService _lessonService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Post(LessonRequest request)
    {
        var response = await _lessonService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _lessonService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _lessonService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Put(Guid id, LessonRequest request)
    {
        var response = await _lessonService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _lessonService.RemoveAsync(id); 
        return Ok(response);
    }
}