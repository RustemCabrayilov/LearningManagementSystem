using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentGroup;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class StudentGroupsController(IStudentGroupService _studentGroupService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<StudentGroupDto>))]
    public async Task<IActionResult> Post(StudentGroupDto request)
    {
        var response = await _studentGroupService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _studentGroupService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    [ServiceFilter(typeof(EntityExistFilter<StudentGroup>))]

    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _studentGroupService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<StudentGroupDto>))]

    public async Task<IActionResult> Put(Guid id, StudentGroupDto request)
    {
        var response = await _studentGroupService.UpdateAsync(id, request); 
        return Ok(response);
    }
    /*[HttpPut("studentExam-list")]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Put(StudentGroupDto[] requests)
    {
        var response = await _studentGroupService.UpdateRangeAsync(requests); 
        return Ok(response);
    }*/
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _studentGroupService.RemoveAsync(id); 
        return Ok(response);
    }
}