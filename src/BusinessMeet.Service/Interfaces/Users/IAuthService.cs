using BusinessMeet.Service.DTOs.Logins;

namespace BusinessMeet.Service.Interfaces.Users;

public interface IAuthService
{
    public Task<LoginForResultDto> AuthenticateAsync(LoginDto login);

}
