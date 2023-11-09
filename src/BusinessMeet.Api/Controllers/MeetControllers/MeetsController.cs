using BusinessMeet.Api.Controllersl;
using BusinessMeet.Api.Moddels;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.Meets;
using BusinessMeet.Service.DTOs.Users;
using BusinessMeet.Service.Interfaces.Meet;
using BusinessMeet.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace BusinessMeet.Api.Controllers.MeetControllers;

public class MeetsController : BaseController
{
    private readonly IMeetService _meetService;

    public MeetsController(IMeetService meetService)
    {
        _meetService = meetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data =await _meetService.RetrieveAllAsync(@params)
        };
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] long id)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data =await _meetService.RetrieveByIdAsync(id)
        };

        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromForm] MeetForCreationDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _meetService.AddAsync(dto)
        };
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _meetService.RemoveAsync(id)
        };
        return Ok(response);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long Id, [FromForm] MeetForUpdateDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _meetService.ModifyAsync(Id, dto)
        };

        return Ok(response);
    }
}
