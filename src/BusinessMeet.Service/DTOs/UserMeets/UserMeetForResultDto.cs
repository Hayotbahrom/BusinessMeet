using BusinessMeet.Service.DTOs.Meets;
using BusinessMeet.Service.DTOs.Users;

namespace BusinessMeet.Service.DTOs.UserMeets;

public class UserMeetForResultDto
{
    public long Id { get; set; }
    public UserForResultDto User {  get; set; }
    public MeetForResultDto Meet { get; set; }

}
