using AutoMapper;
using BusinessMeet.Data.IRepositories;
using BusinessMeet.Domain.Entities;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.UserMeets;
using BusinessMeet.Service.Exceptions;
using BusinessMeet.Service.Extensions;
using BusinessMeet.Service.Interfaces.UserMeets;
using Microsoft.EntityFrameworkCore;

namespace BusinessMeet.Service.Services.UserMeets;

public class UserMeetService : IUserMeetService
{
    private readonly IMapper _mapper;
    private readonly IRepository<UserMeet> _repository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Meet> _meetRepository;

    public UserMeetService(IMapper mapper,
        IRepository<UserMeet> repository,
        IRepository<User> userRepository,
        IRepository<Meet> meetRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _userRepository = userRepository;
        _meetRepository = meetRepository;
    }
    public async Task<UserMeetForResultDto> AddAsync(UserMeetForCreationDto dto)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == dto.UserId)
            .FirstOrDefaultAsync();
        if (user is null)
            throw new BusinessMeetException(404, "User is not found");

        var meet = await _meetRepository.SelectAll()
            .Where(m => m.Id == dto.MeetId)
            .FirstOrDefaultAsync();
        if (meet is null)
            throw new BusinessMeetException(404, "Meet is not found");

        var userMeet = await _repository.SelectAll()
            .Where(uM => uM.UserId == dto.UserId && uM.MeetId == dto.MeetId)
            .FirstOrDefaultAsync(); 

        if (userMeet is not null)
            throw new BusinessMeetException(409, "UserMeet is alrady exist");

        var mapped = _mapper.Map<UserMeet>(dto);
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _repository.CreateAsync(mapped);

        return _mapper.Map<UserMeetForResultDto>(result);
    }

    public async Task<UserMeetForResultDto> ModifyAsync(long id, UserMeetForUpdateDto dto)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == dto.UserId)
            .FirstOrDefaultAsync();
        if (user is null)
            throw new BusinessMeetException(404, "User is not found");

        var meet = await _meetRepository.SelectAll()
            .Where(m => m.Id == dto.MeetId)
            .FirstOrDefaultAsync();
        if (meet is null)
            throw new BusinessMeetException(404, "Meet is not found");

        var userMeet = await _repository.SelectAll()
            .Where(uM => uM.Id == id)
            .FirstOrDefaultAsync();

        if (userMeet is null)
            throw new BusinessMeetException(404, "UserMeet is not found");

        var mapped = _mapper.Map(dto, userMeet);
        mapped.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(mapped);

        return _mapper.Map<UserMeetForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var userMeet = await _repository.SelectAll()
            .Where(uM => uM.Id == id)
            .FirstOrDefaultAsync();

        if (userMeet is null)
            throw new BusinessMeetException(404, "UserMeet is not found");

        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserMeetForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var userMeets = _repository.SelectAll()
            .Include(u => u.User)
            .Include(m => m.Meet)
            .AsNoTracking()
            .ToPagedList(@params);

        return _mapper.Map<IEnumerable<UserMeetForResultDto>>(userMeets);
    }

    public async Task<UserMeetForResultDto> RetrieveByIdAsync(long id)
    {
        var userMeet = await _repository.SelectAll()
            .Where(uM => uM.Id == id)
            .Include(u => u.User)
            .Include(m => m.Meet)
            .FirstOrDefaultAsync();

        if (userMeet is null)
            throw new BusinessMeetException(404, "UserMeet is not found");

        return _mapper.Map<UserMeetForResultDto>(userMeet);
    }
}
