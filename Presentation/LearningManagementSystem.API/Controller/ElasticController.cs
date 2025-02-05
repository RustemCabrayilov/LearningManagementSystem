using LearningManagementSystem.Application.Abstractions.Services.ElasticService;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.OCR;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class ElasticController(IElasticService _elasticService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Post(UserResponse request)
    {
        var response = await _elasticService.AddOrUpdateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]string name, [FromQuery]string value)
    {
        var response = await _elasticService.GetAll(name, value); 
        return Ok(response);
    }
    [HttpGet("{key}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromRoute]string key)
    {
        var response = await _elasticService.Get(key); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Delete(string key)
    {
        var response = await _elasticService.Remove(key); 
        return Ok(response);
    }
    [HttpDelete("delete-all")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> DeleteAll()
    {
        var response = await _elasticService.RemoveAll(); 
        return Ok(response);
    }
    [HttpDelete("delete-index/{indexname}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> DeleteIndex(string indexname)
    {
        var response = await _elasticService.RemoveIndexAsync(indexname); 
        return Ok(response);
    }
}