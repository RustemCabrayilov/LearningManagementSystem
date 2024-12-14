using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class MajorsController (IMajorService _majorService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(MajorRequest request)
    {
        var response = await _majorService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _majorService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _majorService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, MajorRequest request)
    {
        var response = await _majorService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _majorService.RemoveAsync(id); 
        return Ok(response);
    }
}