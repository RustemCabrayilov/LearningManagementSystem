using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class DocumentsController(IDocumentService _documentService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Post(DocumentRequest request)
    {
        var response = await _documentService.CreateAsync(request);
        return Ok(response);
    }
    [HttpPost("create-byowner")]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<DocumentByOwner>))]
    public async Task<IActionResult> Post([FromForm]DocumentByOwner documentByOwner)
    {
        var response = await _documentService.CreateByOwnerAsync(documentByOwner);
            
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _documentService.GetAllAsync(filter);
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    [ServiceFilter(typeof(EntityExistFilter<Document>))]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _documentService.GetAsync(id);
        return Ok(response);
    }
    [HttpGet("get-byowner/{ownerId}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> GetByOwner(Guid ownerId)
    {
        var response = await _documentService.GetByOwnerId(ownerId);
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<DocumentRequest>))]
    public async Task<IActionResult> Put(Guid id,DocumentRequest request)
    {
        var response = await _documentService.UpdateAsync(id,request);
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _documentService.RemoveAsync(id);
        return Ok(response);
    }
}