using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class TeachersController(ITeacherService _teacherService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<TeacherRequest>))]

    public async Task<IActionResult> Post([FromForm]TeacherRequest request)
    {
        var response = await _teacherService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _teacherService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    [ServiceFilter(typeof(EntityExistFilter<Teacher>))]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _teacherService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<TeacherRequest>))]
    public async Task<IActionResult> Put(Guid id, [FromForm]TeacherRequest request)
    {
        var response = await _teacherService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _teacherService.RemoveAsync(id); 
        return Ok(response);
    }
}