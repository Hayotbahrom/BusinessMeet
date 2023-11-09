using BusinessMeet.Service.DTOs.FileUploads;

namespace BusinessMeet.Service.Interfaces.IFileUploadServices;

public interface IFileUploadService
{
    public Task<FileUploadForResultDto> FileUploadAsync(FileUploadForCreationDto dto);
    public Task<bool> FileDeleteAsync(string filePath);
}
