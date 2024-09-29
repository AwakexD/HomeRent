using Microsoft.AspNetCore.Http;

namespace HomeRent.Services.Contracts
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);

        Task<bool> DeleteImageAsync(List<string> imagePublicIds);
    }
}
