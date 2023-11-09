using BusinessMeet.Domain.Commons;

namespace BusinessMeet.Domain.Entities;

public class Meet : Auditable
{
    public long UserId {  get; set; }
    public User User { get; set; }

    public long CompanyId { get; set; }
    public Company Company { get; set; }

    public string Name { get; set; }
    public DateTime Time { get; set; }
    public string Image {  get; set; }
    public string Address { get; set; }

}
