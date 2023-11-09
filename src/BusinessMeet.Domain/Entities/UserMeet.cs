using BusinessMeet.Domain.Commons;

namespace BusinessMeet.Domain.Entities;

public class UserMeet :Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }

    public long MeetId { get; set; }
    public Meet Meet { get; set; }

}
