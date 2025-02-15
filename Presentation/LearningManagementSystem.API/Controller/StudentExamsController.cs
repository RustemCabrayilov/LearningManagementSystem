using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class StudentExamsController(IStudentExamService _studentExamService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<StudentExamRequest>))]

    public async Task<IActionResult> Post(StudentExamRequest request)
    {
        var response = await _studentExamService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _studentExamService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    [ServiceFilter(typeof(EntityExistFilter<StudentExam>))]

    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _studentExamService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<StudentExamRequest>))]
    public async Task<IActionResult> Put(Guid id, StudentExamRequest request)
    {
        var response = await _studentExamService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpPut("studentExam-list")]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Put(StudentExamRequest[] requests)
    {
        var response = await _studentExamService.UpdateRangeAsync(requests); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _studentExamService.RemoveAsync(id); 
        return Ok(response);
    }
}