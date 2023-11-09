using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.UserMeets;
using BusinessMeet.Service.DTOs.Users;

namespace BusinessMeet.Service.Interfaces.UserMeets;

public interface IUserMeetService
{
    Task<bool> RemoveAsync(long id);
    Task<UserMeetForResultDto> RetrieveByIdAsync(long id);
    Task<IEnumerable<UserMeetForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<UserMeetForResultDto> AddAsync(UserMeetForCreationDto dto);
    Task<UserMeetForResultDto> ModifyAsync(long id, UserMeetForUpdateDto dto);
}
