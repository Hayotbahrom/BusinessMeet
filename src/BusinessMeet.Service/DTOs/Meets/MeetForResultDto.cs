using BusinessMeet.Service.DTOs.Companys;
using BusinessMeet.Service.DTOs.Users;

namespace BusinessMeet.Service.DTOs.Meets;

public class MeetForResultDto
{
    public long Id { get; set; }
    public UserForResultDto User { get; set; }
    public CompanyForResultDto Company { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
    public string Image {  get; set; }
    public string Address { get; set; }
}
