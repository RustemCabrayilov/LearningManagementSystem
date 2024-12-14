using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class DocumentsController(IDocumentService _documentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(DocumentRequest request)
    {
        var response = await _documentService.CreateAsync(request);
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _documentService.GetAllAsync(filter);
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _documentService.GetAsync(id);
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id,DocumentRequest request)
    {
        var response = await _documentService.UpdateAsync(id,request);
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _documentService.RemoveAsync(id);
        return Ok(response);
    }
}