using Microsoft.AspNetCore.Http;

namespace BusinessMeet.Service.DTOs.Meets;

public class MeetForUpdateDto
{
    public long UserId { get; set; }
    public long CompanyId { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
    public IFormFile Image { get; set; }
    public string Address { get; set; }
}
