using AutoMapper;
using WebAPI.DTOs.Products;
using WebAPI.DTOs.Products.Create;
using WebAPI.DTOs.Products.Update;
using WebAPI.Models;

namespace WebAPI.Mappings
{
    public class ProductMP : Profile
    {
        public ProductMP()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.PublicId, opt => opt.Ignore())
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.PublicId, opt => opt.Ignore())
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())

                .ForMember(dest => dest.ProductName, opt =>
                {
                    opt.PreCondition(src => src.ProductName != null);
                    opt.MapFrom(src => src.ProductName);
                })

                .ForMember(dest => dest.CategoryId, opt =>
                {
                    opt.PreCondition(src => src.CategoryId.HasValue);
                    opt.MapFrom(src => src.CategoryId!.Value);
                })

                .ForMember(dest => dest.Price, opt =>
                {
                    opt.PreCondition(src => src.Price.HasValue);
                    opt.MapFrom(src => src.Price!.Value);
                })

                .ForMember(dest => dest.ImageUrl, opt =>
                {
                    opt.PreCondition(src => src.ImageUrl != null);
                    opt.MapFrom(src => src.ImageUrl);
                })

                .ForMember(dest => dest.Description, opt =>
                {
                    opt.PreCondition(src => src.Description != null);
                    opt.MapFrom(src => src.Description);
                });
            CreateMap<Product, UpdateProductDTO>();

        }
    }
}
