using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class GroupsController (IGroupService _groupService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(GroupRequest request)
    {
        var response = await _groupService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _groupService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _groupService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, GroupRequest request)
    {
        var response = await _groupService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _groupService.RemoveAsync(id); 
        return Ok(response);
    }
}