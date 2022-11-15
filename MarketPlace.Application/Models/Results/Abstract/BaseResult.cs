namespace MarketPlace.Application.Models.Results.Abstract;

public abstract class BaseResult
{
    public string Id { get; set; } = null!;
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}