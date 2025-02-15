using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class StudentRetakeExamsController(IStudentRetakeExamService _studentRetakeExamService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<StudentRetakeExamDto>))]

    public async Task<IActionResult> Post(StudentRetakeExamDto request)
    {
        var response = await _studentRetakeExamService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _studentRetakeExamService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("active-term-requests")]
    [Authorize(Roles = "Admin,Dean,Student")]
    public async Task<IActionResult> ActiveTermRequest([FromQuery]RequestFilter? filter)
    {
        var response = await _studentRetakeExamService.ActiveTermRetakeRequests(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Student")]
    [ServiceFilter(typeof(EntityExistFilter<StudentRetakeExam>))]

    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _studentRetakeExamService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<StudentRetakeExamDto>))]
    public async Task<IActionResult> Put(Guid id, StudentRetakeExamDto request)
    {
        var response = await _studentRetakeExamService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _studentRetakeExamService.RemoveAsync(id); 
        return Ok(response);
    }
}