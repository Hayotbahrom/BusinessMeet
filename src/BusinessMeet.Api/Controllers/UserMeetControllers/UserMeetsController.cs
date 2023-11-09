using BusinessMeet.Api.Controllersl;
using BusinessMeet.Api.Moddels;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.UserMeets;
using BusinessMeet.Service.DTOs.Users;
using BusinessMeet.Service.Interfaces.UserMeets;
using BusinessMeet.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace BusinessMeet.Api.Controllers.UserMeetControllers;

public class UserMeetsController : BaseController
{
    private readonly IUserMeetService _userMeetService;

    public UserMeetsController(IUserMeetService userMeetService)
    {
        _userMeetService = userMeetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userMeetService.RetrieveAllAsync(@params)
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
            Data = await _userMeetService.RetrieveByIdAsync(id)
        };

        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] UserMeetForCreationDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userMeetService.AddAsync(dto)
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
            Data = await _userMeetService.RemoveAsync(id)
        };
        return Ok(response);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long Id, [FromBody] UserMeetForUpdateDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userMeetService.ModifyAsync(Id, dto)
        };

        return Ok(response);
    }
}
