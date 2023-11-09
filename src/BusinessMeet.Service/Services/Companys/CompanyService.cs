using AutoMapper;
using BusinessMeet.Data.IRepositories;
using BusinessMeet.Domain.Entities;
using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.Companys;
using BusinessMeet.Service.DTOs.FileUploads;
using BusinessMeet.Service.DTOs.Users;
using BusinessMeet.Service.Exceptions;
using BusinessMeet.Service.Extensions;
using BusinessMeet.Service.Interfaces.Companys;
using BusinessMeet.Service.Interfaces.IFileUploadServices;
using Microsoft.EntityFrameworkCore;

namespace BusinessMeet.Service.Services.Companys;

public class CompanyService : ICompanyService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Company> _repository;
    private readonly IFileUploadService _fileUploadService;

    public CompanyService(
        IMapper mapper,
        IRepository<Company> repository,
        IFileUploadService fileUploadService)
    {
        _mapper = mapper;
        _repository = repository;
        _fileUploadService = fileUploadService;
    }
    public async Task<CompanyForResultDto> AddAsync(CompanyForCreationDto dto)
    {
        var company = await _repository.SelectAll()
            .Where(c => c.Email == dto.Email)
            .FirstOrDefaultAsync();

        if (company is not null)
            throw new BusinessMeetException(409, "Company is already exist");

        var FileUploadForCreation = new FileUploadForCreationDto
        {
            FolderPath = "CompanyAssets",
            FormFile = dto.Image
        };
        var FileResult = await _fileUploadService.FileUploadAsync(FileUploadForCreation);

        var mapped = _mapper.Map<Company>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Image = FileResult.AssetPath;
        var result = await _repository.CreateAsync(mapped);

        return _mapper.Map<CompanyForResultDto>(result);
    }

    public async Task<CompanyForResultDto> ModifyAsync(long id, CompanyForUpdateDto dto)
    {
        var company = await _repository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (company is null)
            throw new BusinessMeetException(404, "Company is not found");
        //delete image
        await _fileUploadService.FileDeleteAsync(company.Image);

        // update image
        var FileUploadForCreation = new FileUploadForCreationDto
        {
            FolderPath = "CompanyAssets",
            FormFile = dto.Image
        };
        var FileResult = await _fileUploadService.FileUploadAsync(FileUploadForCreation);

        var mapped = _mapper.Map(dto, company);
        mapped.UpdatedAt = DateTime.UtcNow;
        mapped.Image = FileResult.AssetPath;
        await _repository.UpdateAsync(mapped);

        return _mapper.Map<CompanyForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var company = await _repository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (company is null)
            throw new BusinessMeetException(404, "Company is not found");

        await _fileUploadService.FileDeleteAsync(company.Image);

        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CompanyForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var companies = _repository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params);

        foreach (var company in companies)
        {
            company.Image = $"https://localhost:7236/{company.Image.Replace('\\', '/').TrimStart('/')}";
        }

        return _mapper.Map<IEnumerable<CompanyForResultDto>>(companies);
    }

    public async Task<CompanyForResultDto> RetrieveByIdAsync(long id)
    {
        var company = await _repository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (company is null)
            throw new BusinessMeetException(404, "Company is not found");

        company.Image = $"https://localhost:7236/{company.Image.Replace('\\', '/').TrimStart('/')}";

        return _mapper.Map<CompanyForResultDto>(company);
    }
}
