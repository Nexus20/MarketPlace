namespace MarketPlace.Application.Models.Requests.Products;

public class GetProductsRequest
{
    public string? SearchString { get; set; }
    public string? ShopId { get; set; }
}