using AutoMapper;
using MarketPlace.Application.Models.Requests.Products;
using MarketPlace.Application.Models.Results.Products;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();
        CreateMap<Product, ProductResult>()
            .ForMember(x => x.Categories, o => o.MapFrom(s => s.ProductCategories.Select(pc => pc.Category)));
    }
}