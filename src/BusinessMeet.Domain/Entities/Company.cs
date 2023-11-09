using BusinessMeet.Domain.Commons;

namespace BusinessMeet.Domain.Entities;

public class Company : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Direction { get; set; }
    public string Image {  get; set; }
    public ICollection<Meet> Meets { get; set; }
}
