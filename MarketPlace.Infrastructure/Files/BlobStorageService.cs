using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MarketPlace.Application.Interfaces.Infrastructure;
using MarketPlace.Application.Models.Dtos;

namespace MarketPlace.Infrastructure.Files;

public class BlobStorageService : IFileStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<UrlsDto> UploadAsync(List<FileDto> files)
    {
        if (files == null || files.Count == 0)
            return null;

        var containerClient = _blobServiceClient.GetBlobContainerClient("market-place-images");

        var urls = new List<string>();

        foreach (var file in files)
        {
            var blobClient = containerClient.GetBlobClient(file.GetPathWithFileName());
            await blobClient.UploadAsync(file.Content, new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            });
            
            urls.Add(blobClient.Uri.ToString());
        }

        return new UrlsDto(urls);
    }
}