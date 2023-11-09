using BusinessMeet.Api.Controllersl;
using BusinessMeet.Api.Moddels;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.Users;
using BusinessMeet.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace BusinessMeet.Api.Controllers.UserControllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RetrieveAllAsync(@params)
        };
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute(Name = "id")]long id)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RetrieveByIdAsync(id)
        };

        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromForm]UserForCreationDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.AddAsync(dto)
        };
        return Ok(response);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")]long id)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data =await _userService.RemoveAsync(id)
        };
        return Ok(response);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long Id,[FromForm] UserForUpdateDto dto)
    {
        var response = new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data =await _userService.ModifyAsync(Id, dto)
        };

        return Ok(response);
    }
}
