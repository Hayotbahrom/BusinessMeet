using BusinessMeet.Domain.Entities;

namespace BusinessMeet.Service.Interfaces.IEmailServices;

public interface IEmailService
{
    public Task SendEmail(Message message);
}
