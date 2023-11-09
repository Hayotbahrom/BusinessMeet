using Microsoft.AspNetCore.Http;

namespace BusinessMeet.Service.DTOs.FileUploads;

public class FileUploadForCreationDto
{
    public string FolderPath { get; set; }
    public IFormFile FormFile { get; set; }
}
