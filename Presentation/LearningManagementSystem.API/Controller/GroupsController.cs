using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class GroupsController (IGroupService _groupService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Post(GroupRequest request)
    {
        var response = await _groupService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _groupService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _groupService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Put(Guid id, GroupRequest request)
    {
        var response = await _groupService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _groupService.RemoveAsync(id); 
        return Ok(response);
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Post(Guid id)
    {
        var response = await _groupService.Activate(id);
        return Ok(response);
    } 
}