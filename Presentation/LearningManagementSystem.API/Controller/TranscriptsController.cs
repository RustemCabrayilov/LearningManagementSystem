using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class TranscriptsController(ITranscriptService _transcriptService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Post(TranscriptRequest request)
    {
        var response = await _transcriptService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _transcriptService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _transcriptService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Put(Guid id, TranscriptRequest request)
    {
        var response = await _transcriptService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _transcriptService.RemoveAsync(id); 
        return Ok(response);
    }
}