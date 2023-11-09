using BusinessMeet.Api.Controllersl;
using BusinessMeet.Service.DTOs.Logins;
using BusinessMeet.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace BusinessMeet.Api.Controllers.AuthControllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Authenticate")]
    public async Task<IActionResult> PostAsync(LoginDto dto)
    {
        var token = await this._authService.AuthenticateAsync(dto);
        return Ok(token);
    }
}
