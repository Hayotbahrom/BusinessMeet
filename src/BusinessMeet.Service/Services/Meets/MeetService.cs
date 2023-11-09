using AutoMapper;
using BusinessMeet.Data.IRepositories;
using BusinessMeet.Domain.Entities;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.FileUploads;
using BusinessMeet.Service.DTOs.Meets;
using BusinessMeet.Service.DTOs.Users;
using BusinessMeet.Service.Exceptions;
using BusinessMeet.Service.Extensions;
using BusinessMeet.Service.Interfaces.Companys;
using BusinessMeet.Service.Interfaces.IEmailServices;
using BusinessMeet.Service.Interfaces.IFileUploadServices;
using BusinessMeet.Service.Interfaces.Meet;
using Microsoft.EntityFrameworkCore;

namespace BusinessMeet.Service.Services.Meets;

public class MeetService : IMeetService
{
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IRepository<Meet> _repository;
    private readonly IRepository<User> _userRepository;
    private readonly IFileUploadService _uploadService;
    private readonly IRepository<Company> _companyRepository;
    public MeetService(IMapper mapper,
        IEmailService emailService,
        IRepository<Meet> repository, 
        IFileUploadService uploadService,
        IRepository<User> userRepository,
        IRepository<Company> companyRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _emailService = emailService;
        _uploadService = uploadService;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
    }
    public async Task<MeetForResultDto> AddAsync(MeetForCreationDto dto)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == dto.UserId)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new BusinessMeetException(404, "User is not found");

        var company = await _companyRepository.SelectAll()
            .Where(c => c.Id == dto.CompanyId)
            .FirstOrDefaultAsync();

        if (company == null)
            throw new BusinessMeetException(404, "Company is not found");

        var meet = await _repository.SelectAll()
            .Where(m => m.UserId == dto.UserId &&
            m.CompanyId == dto.CompanyId &&
            m.Address == dto.Address &&
            m.Name == dto.Name)
            .FirstOrDefaultAsync();

        if (meet is not null)
            throw new BusinessMeetException(409, "Meet is already exist");

        var FileUploadForCreation = new FileUploadForCreationDto
        {
            FolderPath = "MeetAssets",
            FormFile = dto.Image
        };
        var FileResult = await _uploadService.FileUploadAsync(FileUploadForCreation);

        var mapped = _mapper.Map<Meet>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Image = FileResult.AssetPath;
        var result = await _repository.CreateAsync(mapped);


        Message message = new Message()
        {
            Subject = "Business Meeting Schedule",
            Body = $" Dear {user.FirstName},\r\n\r\nI hope this message finds you well. We would like to inform you of the upcoming business meeting schedule for {mapped.Time.Hour}.\r\n\r\nMeeting Details:\r\n\r\nDate: {mapped.Time.Date}\r\nLocation: {mapped.Address}\n" +
            $"If you are unable to attend, please inform us at your earliest convenience so that we can make necessary arrangements.\r\n\r\nWe look forward to productive discussions and valuable insights during the meeting.\r\n\r\nShould you have any questions or require further information, please do not hesitate to reach out.\r\n\r\nThank you, and we anticipate a successful and fruitful meeting.\r\n\r\nBest regards,\r\n\r\n{company.Name}\r\n{company.Email}",
            To = user.Email
        };
       
        await _emailService.SendEmail(message);

        return _mapper.Map<MeetForResultDto>(result);
    }

    public async Task<MeetForResultDto> ModifyAsync(long id, MeetForUpdateDto dto)
    {
        var user = await _userRepository.SelectAll()
            .Where(u => u.Id == dto.UserId)
            .FirstOrDefaultAsync();
        if (user == null)
            throw new BusinessMeetException(404, "User is not found");

        var company = await _companyRepository.SelectAll()
            .Where(c => c.Id == dto.CompanyId)
            .FirstOrDefaultAsync();

        if (company == null)
            throw new BusinessMeetException(404, "Company is not found");

        var meet = await _repository.SelectAll()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();

        if (meet is null)
            throw new BusinessMeetException(404, "Meet is not found");
        //delete
        await _uploadService.FileDeleteAsync(meet.Image);
        //upload
        var FileUploadForCreation = new FileUploadForCreationDto
        {
            FolderPath = "MeetAssets",
            FormFile = dto.Image
        };
        var FileResult = await _uploadService.FileUploadAsync(FileUploadForCreation);

        var mapped = _mapper.Map(dto, meet);
        mapped.UpdatedAt = DateTime.UtcNow;
        mapped.Image = FileResult.AssetPath;
        await _repository.UpdateAsync(mapped);

        Message message = new Message()
        {
            Subject = "Business Meeting Schedule",
            Body = $" Dear {user.FirstName},\r\n\r\nI hope this message finds you well. We would like to inform you of the upcoming business meeting schedule for {mapped.Time.Hour}.\r\n\r\nMeeting Details:\r\n\r\nDate: {mapped.Time.Date}\r\nLocation: {mapped.Address}\n" +
            $"If you are unable to attend, please inform us at your earliest convenience so that we can make necessary arrangements.\r\n\r\nWe look forward to productive discussions and valuable insights during the meeting.\r\n\r\nShould you have any questions or require further information, please do not hesitate to reach out.\r\n\r\nThank you, and we anticipate a successful and fruitful meeting.\r\n\r\nBest regards,\r\n\r\n{company.Name}\r\n{company.Email}",
            To = user.Email
        };

        await _emailService.SendEmail(message);

        return _mapper.Map<MeetForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var meet = await _repository.SelectAll()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
        if (meet is null)
            throw new BusinessMeetException(404, "Meet is not null");

        await _uploadService.FileDeleteAsync(meet.Image);

        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MeetForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var meets = _repository.SelectAll()
            .Include(u => u.User)
            .Include(c => c.Company)
            .AsNoTracking()
            .ToPagedList(@params);

        foreach (var meet in meets)
        {
            meet.Image = $"https://localhost:7236/{meet.Image.Replace('\\', '/').TrimStart('/')}";
        }

        return _mapper.Map<IEnumerable<MeetForResultDto>>(meets);
    }

    public async Task<MeetForResultDto> RetrieveByIdAsync(long id)
    {
        var meet = await _repository.SelectAll()
            .Where (m => m.Id == id)
            .Include(u => u.User)
            .Include(c => c.Company)
            .FirstOrDefaultAsync();

        if (meet is null)
            throw new BusinessMeetException(404, "Meet is not found");
        meet.Image = $"https://localhost:7236/{meet.Image.Replace('\\', '/').TrimStart('/')}";

        return _mapper.Map<MeetForResultDto>(meet);
    }
}
