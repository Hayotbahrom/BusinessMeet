using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.Meets;
using BusinessMeet.Service.DTOs.Users;

namespace BusinessMeet.Service.Interfaces.Meet;

public interface IMeetService
{
    Task<bool> RemoveAsync(long id);
    Task<MeetForResultDto> RetrieveByIdAsync(long id);
    Task<IEnumerable<MeetForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<MeetForResultDto> AddAsync(MeetForCreationDto dto);
    Task<MeetForResultDto> ModifyAsync(long id, MeetForUpdateDto dto);
}
