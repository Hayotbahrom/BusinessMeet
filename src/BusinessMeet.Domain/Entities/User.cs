using BusinessMeet.Domain.Commons;
using BusinessMeet.Domain.Enums;

namespace BusinessMeet.Domain.Entities;

public class User : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Image { get; set; }
    public string Address { get; set; }
    public UserRole Role { get; set; }

}
