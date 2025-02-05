using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class DeansController(IDeanService _deanService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<DeanRequest>))]
    public async Task<IActionResult> Post([FromForm]DeanRequest request)
    {
        var response = await _deanService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _deanService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    [ServiceFilter(typeof(EntityExistFilter<Dean>))]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _deanService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Put(Guid id, [FromForm]DeanRequest request)
    {
        var response = await _deanService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _deanService.RemoveAsync(id); 
        return Ok(response);
    }
    /*[HttpPost("assign-group")]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Post(TeacherGroupDto request)
    {
        var response = await _deanService.AssignGroupAsync(request); 
        return Ok(response);
    }*/
}