using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class QuestionsController(IQuestionService _questionService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<QuestionRequest>))]
    public async Task<IActionResult> Post(QuestionRequest request)
    {
        var response = await _questionService.CreateAsync(request);
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _questionService.GetAllAsync(filter);
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    [ServiceFilter(typeof(EntityExistFilter<Question>))]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _questionService.GetAsync(id);
        return Ok(response);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Put(Guid id, QuestionRequest request)
    {
        var response = await _questionService.UpdateAsync(id,request);
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _questionService.RemoveAsync(id);
        return Ok(response);
    }
}