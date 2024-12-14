using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Student;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentService _studentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(StudentRequest request)
    {
        var response = await _studentService.CreateAsync(request);
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _studentService.GetAllAsync(filter);
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _studentService.GetAsync(id);
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, StudentRequest request)
    {
        var response = await _studentService.UpdateAsync(id,request);
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _studentService.RemoveAsync(id);
        return Ok(response);
    }
    [HttpPost("assign-group")]
    public async Task<IActionResult> Post(StudentGroupDto request)
    {
        var response = await _studentService.AssignGroupAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-lesson")]
    public async Task<IActionResult> Post(StudentLessonDto request)
    {
        var response = await _studentService.AssignLessonAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-major")]
    public async Task<IActionResult> Post(StudentMajorDto request)
    {
        var response = await _studentService.AssignMajorAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-subject")]
    public async Task<IActionResult> Post(StudentSubjectDto request)
    {
        var response = await _studentService.AssignSubjectAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-exam")]
    public async Task<IActionResult> Post(StudentExamDto request)
    {
        var response = await _studentService.AssignExamAsync(request); 
        return Ok(response);
    }
    [HttpPost("assign-retakeExam")]
    public async Task<IActionResult> Post(StudentRetakeExamDto request)
    {
        var response = await _studentService.AssignRetakeExamAsync(request); 
        return Ok(response);
    }
}