using LearningManagementSystem.Application.Abstractions.Services.Role;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;

public class RolesController(IRoleService _roleService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(RoleRequest request)
    {
        var response = await _roleService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _roleService.GetAllAsync(); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(string id)
    {
        var response = await _roleService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(string id, RoleRequest request)
    {
        var response = await _roleService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _roleService.RemoveAsync(id); 
        return Ok(response);
    }
}