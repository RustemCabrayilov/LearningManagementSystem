using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class AttendancesController(IAttendanceService _attendanceService) : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<AttendanceRequest>))]

    public async Task<IActionResult> Post(AttendanceRequest request)
    {
        var response = await _attendanceService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> Get([FromQuery]RequestFilter? filter)
    {
        var response = await _attendanceService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    [ServiceFilter(typeof(EntityExistFilter<Attendance>))]
    public async Task<IActionResult> Get([FromRoute]Guid id)
    {
        var response = await _attendanceService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<AttendanceRequest>))]
    public async Task<IActionResult> Put(Guid id, AttendanceRequest request)
    {
        var response = await _attendanceService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpPut("attendance-list")]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(CollectionFilter<AttendanceRequest>))]

    public async Task<IActionResult> Put(AttendanceRequest[] requests)
    {
        var response = await _attendanceService.UpdateRangeAsync(requests); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _attendanceService.RemoveAsync(id); 
        return Ok(response);
    }

    [HttpPost("student-attendance-list")]
    [Authorize(Roles = "Admin,Dean,Teacher,Student")]
    public async Task<IActionResult> GetStudentAttendanceList(StudentAttendanceDto request)
    {
        var response = await _attendanceService.StudentAttendances(request);
        return Ok(response);
    }
    
}