using BusinessMeet.Service.Configurations;
using BusinessMeet.Service.DTOs.Companys;

namespace BusinessMeet.Service.Interfaces.Companys;

public interface ICompanyService
{
    Task<bool> RemoveAsync(long id);
    Task<CompanyForResultDto> RetrieveByIdAsync(long id);
    Task<IEnumerable<CompanyForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<CompanyForResultDto> AddAsync(CompanyForCreationDto dto);
    Task<CompanyForResultDto> ModifyAsync(long id, CompanyForUpdateDto dto);
}
