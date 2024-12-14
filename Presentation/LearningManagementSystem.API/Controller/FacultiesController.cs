using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class FacultiesController(IFacultyService _facultyService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(FacultyRequest request)
    {
        var response = await _facultyService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _facultyService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _facultyService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, FacultyRequest request)
    {
        var response = await _facultyService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _facultyService.RemoveAsync(id); 
        return Ok(response);
    }
}