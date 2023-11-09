using AutoMapper;
using BusinessMeet.Data.IRepositories;
using BusinessMeet.Domain.Entities;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.FileUploads;
using BusinessMeet.Service.DTOs.Users;
using BusinessMeet.Service.Exceptions;
using BusinessMeet.Service.Extensions;
using BusinessMeet.Service.Interfaces.IFileUploadServices;
using BusinessMeet.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace BusinessMeet.Service.Services.Users;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IRepository<User> _repository;
    private readonly IFileUploadService _uploadService;
    public UserService(IMapper mapper, IRepository<User> repository, IFileUploadService uploadService)
    {
        _mapper = mapper;
        _repository = repository;
        _uploadService = uploadService;
    }
    public async Task<UserForResultDto> AddAsync(UserForCreationDto dto)
    {
        var user = await _repository.SelectAll()
            .Where(u => u.Email == dto.Email && u.Phone == dto.Phone)
            .FirstOrDefaultAsync();

        if (user is not null)
            throw new BusinessMeetException(409, "User is already exist");

        var FileUploadForCreation = new FileUploadForCreationDto
        {
            FolderPath = "UserAssets",
            FormFile = dto.Image
        };
        var FileResult = await _uploadService.FileUploadAsync(FileUploadForCreation);

        var mapped = _mapper.Map<User>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Image = FileResult.AssetPath;
        var result = await _repository.CreateAsync(mapped);

        return _mapper.Map<UserForResultDto>(result);
    }

    public async Task<UserForResultDto> ModifyAsync(long id, UserForUpdateDto dto)
    {
        var user = await _repository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new BusinessMeetException(404, "User is not found");
        //delete image
        await _uploadService.FileDeleteAsync(user.Image);

        // update image
        var FileUploadForCreation = new FileUploadForCreationDto
        {
            FolderPath = "UserAssets",
            FormFile = dto.Image
        };
        var FileResult = await _uploadService.FileUploadAsync(FileUploadForCreation);

        var mapped = _mapper.Map(dto,user);
        mapped.UpdatedAt = DateTime.UtcNow;
        mapped.Image = FileResult.AssetPath;
        await _repository.UpdateAsync(mapped);

        return _mapper.Map<UserForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var user = await _repository.SelectAll()
            .Where (u => u.Id == id)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new BusinessMeetException(404, "User is not null");

        await _uploadService.FileDeleteAsync (user.Image);

        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = _repository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params);

        foreach (var user in users)
        {
            user.Image = $"https://localhost:7236/{user.Image.Replace('\\', '/').TrimStart('/')}";
        }

        return _mapper.Map<IEnumerable<UserForResultDto>>(users);
    }

    public async Task<UserForResultDto> RetrieveByIdAsync(long id)
    {
        var user = await _repository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new BusinessMeetException(404, "User is not found");

        user.Image = $"https://localhost:7236/{user.Image.Replace('\\', '/').TrimStart('/')}";

        return _mapper.Map<UserForResultDto>(user);
    }
}
