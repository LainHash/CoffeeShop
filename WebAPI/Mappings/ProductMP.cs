using AutoMapper;
using WebAPI.DTOs.Products;
using WebAPI.DTOs.Products.Create;
using WebAPI.Models;

namespace WebAPI.Mappings
{
    public class ProductMP : Profile
    {
        public ProductMP()
        {
            CreateMap<Product, ProductBaseDTO>().ReverseMap();
            CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())   // identity insert
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());  // set thủ công

        }
    }
}
