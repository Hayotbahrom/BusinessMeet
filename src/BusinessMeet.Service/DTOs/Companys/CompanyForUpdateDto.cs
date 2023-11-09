using Microsoft.AspNetCore.Http;

namespace BusinessMeet.Service.DTOs.Companys;

public class CompanyForUpdateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Direction { get; set; }
    public IFormFile Image { get; set; }
}
