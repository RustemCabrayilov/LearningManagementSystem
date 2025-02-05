using Elastic.Clients.Elasticsearch;
using LearningManagementSystem.Application.Abstractions.Services.Theme;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class ThemesController(IThemeService _themeService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Post([FromForm]ThemeRequest request)
    {
        var response = await _themeService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _themeService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _themeService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Put(Guid id, ThemeRequest request)
    {
        var response = await _themeService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _themeService.RemoveAsync(id); 
        return Ok(response);
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Post(Guid id)
    {
        var response = await _themeService.ActivateAsync(id);
        return Ok(response);
    }
}