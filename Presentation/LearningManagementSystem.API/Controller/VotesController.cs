using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class VotesController (IVoteService _voteService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Student")]
    [ServiceFilter(typeof(ValidationFilter<VoteRequest>))]

    public async Task<IActionResult> Post(VoteRequest request)
    {
        var response = await _voteService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpPost("create-vote-list")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Post(VoteRequest[] requests)
    {
        var response = await _voteService.CreateAsync(requests); 
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _voteService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(EntityExistFilter<Vote>))]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _voteService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<VoteRequest>))]

    public async Task<IActionResult> Put(Guid id, VoteRequest request)
    {
        var response = await _voteService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    [Authorize(Roles = "Admin,Dean,Teacher")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _voteService.RemoveAsync(id); 
        return Ok(response);
    }
}