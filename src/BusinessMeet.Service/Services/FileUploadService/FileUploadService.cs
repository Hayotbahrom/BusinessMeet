    using BusinessMeet.Service.DTOs.FileUploads;
    using BusinessMeet.Service.Helpers;
    using BusinessMeet.Service.Interfaces.IFileUploadServices;

    namespace BusinessMeet.Service.Services.FileUploadService;

    public class FileUploadService : IFileUploadService
    {
        public async Task<bool> FileDeleteAsync(string filePath)
        {
            var result = Path.Combine(WebEnvironmentHost.WebRootPath, filePath);
            if (File.Exists(result))
            {
                File.Delete(result);
                return true;
            }
            return false;
        }   
        public async Task<FileUploadForResultDto> FileUploadAsync(FileUploadForCreationDto dto)
        {
            var wwwRootPath = Path.Combine(WebEnvironmentHost.WebRootPath, "Assets", dto.FolderPath);
            var assetsFolderPath = Path.Combine(WebEnvironmentHost.WebRootPath, "Assets");
            var assetPath = Path.Combine(assetsFolderPath, dto.FolderPath);

            if (!Directory.Exists(assetsFolderPath))
            {
                Directory.CreateDirectory(assetsFolderPath);
            }
            if (!Directory.Exists(assetPath))
            {
                Directory.CreateDirectory(assetPath);
            }

            var FileName = Guid.NewGuid().ToString("N") + Path.GetExtension(dto.FormFile.FileName);
            var FullPath = Path.Combine(wwwRootPath, FileName);
            using (var streamFile = File.OpenWrite(FullPath))
            {
                await dto.FormFile.CopyToAsync(streamFile);
            };

            var result = new FileUploadForResultDto()
            {
                AssetPath = Path.Combine("Assets", dto.FolderPath, FileName),
            };
            return result;

        }


    }


