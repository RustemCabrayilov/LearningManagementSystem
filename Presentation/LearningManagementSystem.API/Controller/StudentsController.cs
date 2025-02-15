using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentService _studentService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<StudentRequest>))]
    public async Task<IActionResult> Post([FromForm]StudentRequest request)
    {
        var response = await _studentService.CreateAsync(request);
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _studentService.GetAllAsync(filter);
        return Ok(response);
    }
    [HttpGet("{id}")]
    [ServiceFilter(typeof(EntityExistFilter<Student>))]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _studentService.GetAsync(id);
        return Ok(response);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Dean")]
    [ServiceFilter(typeof(ValidationFilter<StudentRequest>))]
    public async Task<IActionResult> Put(Guid id, [FromForm]StudentRequest request)
    {
        var response = await _studentService.UpdateAsync(id,request);
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _studentService.RemoveAsync(id);
        return Ok(response);
    }
    [HttpPost("assign-group")]
    [Authorize(Roles = "Admin,Dean,Student")]
    public async Task<IActionResult> Post(StudentGroupDto request)
    {
        var response = await _studentService.AssignGroupAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-group-list")]
    [Authorize(Roles = "Admin,Dean,Student")]
    public async Task<IActionResult> Post(StudentGroupDto[] request)
    {
        var response = await _studentService.AssignGroupsAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-subject")]
    [Authorize(Roles = "Admin,Dean,Student")]
    public async Task<IActionResult> Post(StudentSubjectDto request)
    {
        var response = await _studentService.AssignSubjectAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-exam")]
    [Authorize(Roles = "Admin,Dean")]
    public async Task<IActionResult> Post(StudentExamDto request)
    {
        var response = await _studentService.AssignExamAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-retakeExam")]
    [Authorize(Roles = "Admin,Dean,Student")]
    public async Task<IActionResult> Post(StudentRetakeExamDto request)
    {
        var response = await _studentService.AssignRetakeExamAsync(request); 
        return Ok(response);
    }
}