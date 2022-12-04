using MarketPlace.Application.Models.Dtos;

namespace MarketPlace.Application.Interfaces.Infrastructure;

public interface IFileStorageService
{
    Task<UrlsDto> UploadAsync(List<FileDto> files);
}