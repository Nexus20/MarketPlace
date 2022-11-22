using AutoMapper;
using MarketPlace.Application.Models.Requests.Categories;
using MarketPlace.Application.Models.Results.Abstract;
using MarketPlace.Application.Models.Results.Categories;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.Entities.Abstract;

namespace MarketPlace.Application.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<Category, CategoryResult>()
            .ForMember(x => x.Products,
                o => o.MapFrom(s =>
                    s.ProductCategories != null && s.ProductCategories.Any()
                        ? s.ProductCategories.Select(pc => pc.Product)
                        : new List<Product>()));
    }
}