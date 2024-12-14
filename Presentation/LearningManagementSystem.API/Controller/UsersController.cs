using LearningManagementSystem.Application.Abstractions.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;

public class UsersController(IUserService _userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(UserRequest request)
    {
        var response = await _userService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _userService.GetAllAsync(); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(string id)
    {
        var response = await _userService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(string id, UserRequest request)
    {
        var response = await _userService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _userService.RemoveAsync(id); 
        return Ok(response);
    }
}