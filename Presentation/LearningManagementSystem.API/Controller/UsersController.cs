using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.ElasticService;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService _userService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<UserRequest>))]

    public async Task<IActionResult> Post(UserRequest request)
    {
        var response = await _userService.CreateAsync(request); 

        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _userService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(UserExistFilter))]

    public async Task<IActionResult> Get([FromRoute]string id)
    {
        var response = await _userService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Put(string id, UserRequest request)
    {
        var response = await _userService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _userService.RemoveAsync(id); 
        return Ok(response);
    }
    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Post(UserRoleDto request)
    {
        var response = await _userService.AssignRoleAsync(request); 
        return Ok(response);
    }
    [HttpPost("infoby-token")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Post(string token)
    {
        var response = await _userService.GetUserInfosByToken(token); 
        return Ok(response);
    }
}
