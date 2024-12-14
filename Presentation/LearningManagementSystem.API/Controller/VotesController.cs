using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class VotesController (IVoteService _voteService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(VoteRequest request)
    {
        var response = await _voteService.CreateAsync(request); 
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> Get(RequestFilter? filter)
    {
        var response = await _voteService.GetAllAsync(filter); 
        return Ok(response);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(Guid id)
    {
        var response = await _voteService.GetAsync(id); 
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Put(Guid id, VoteRequest request)
    {
        var response = await _voteService.UpdateAsync(id, request); 
        return Ok(response);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _voteService.RemoveAsync(id); 
        return Ok(response);
    }
}