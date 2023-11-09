using BusinessMeet.Data.IRepositories;
using BusinessMeet.Domain.Entities;
using BusinessMeet.Service.DTOs.Logins;
using BusinessMeet.Service.Exceptions;
using BusinessMeet.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessMeet.Service.Services.Users;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<User> _repository;

    public AuthService(IRepository<User> repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }
    public async Task<LoginForResultDto> AuthenticateAsync(LoginDto login)
    {
        var user = await _repository.SelectAll()
            .Where(u => u.Email == login.Email && u.Password == login.Password)
            .FirstOrDefaultAsync();
        if (user is null)
            throw new BusinessMeetException(400, "UserName or Password is Incorrect");

        return new LoginForResultDto
        {
            Token = GenerateToken(user)
        };
    }
    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim("Id", user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.FirstName),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Audience = _configuration["JWT:Audience"],
            Issuer = _configuration["JWT:Issuer"],
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:Expire"])),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
