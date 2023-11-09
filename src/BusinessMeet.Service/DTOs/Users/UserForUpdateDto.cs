using Microsoft.AspNetCore.Http;

namespace BusinessMeet.Service.DTOs.Users;

public class UserForUpdateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public IFormFile Image { get; set; }
    public string Address { get; set; }
}
