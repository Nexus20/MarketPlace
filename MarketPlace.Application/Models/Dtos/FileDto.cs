using MarketPlace.Application.Extensions;

namespace MarketPlace.Application.Models.Dtos;

public class FileDto
{
    public Stream Content { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    
    public string GetPathWithFileName()
    {
        var uniqueRandomFileName = Path.GetRandomFileName();
        var shortClientSideFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Name).TruncateLongString(10);
        var fileExtension = Path.GetExtension(Name);
        var basePath = "product1/images/";

        return basePath + uniqueRandomFileName + "_" + shortClientSideFileNameWithoutExtension + fileExtension;
    }
}

public class UrlsDto
{
    public UrlsDto(List<string> urls)
    {
        Urls = urls;
    }

    public List<string> Urls { get; }
}