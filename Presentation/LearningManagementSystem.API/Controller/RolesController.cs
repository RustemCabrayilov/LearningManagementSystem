using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Role;
using LearningManagementSystem.BLL.Services.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class RolesController(IRoleService _roleService) : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<RoleRequest>))]
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
    [HttpGet("{id}")]
    [ServiceFilter(typeof(RoleExistFilter))]
    public async Task<IActionResult> Get(string id)
    {
        var response = await _roleService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [ServiceFilter(typeof(RoleExistFilter))]
    [ServiceFilter(typeof(RoleValidator<RoleRequest>))]
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