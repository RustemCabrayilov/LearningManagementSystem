using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class ExamsController(IExamService _examService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<ExamRequest>))]
    public async Task<IActionResult> Post(ExamRequest request)
    {
        var response = await _examService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _examService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(EntityExistFilter<Exam>))]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _examService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<ExamRequest>))]
    public async Task<IActionResult> Put(Guid id, ExamRequest request)
    {
        var response = await _examService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _examService.RemoveAsync(id); 
        return Ok(response);
    }
}