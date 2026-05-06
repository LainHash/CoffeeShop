using AutoMapper;
using WebAPI.DTOs.Products;
using WebAPI.Models;

namespace WebAPI.Mappings
{
    public class ProductMP : Profile
    {
        public ProductMP() 
        {
            CreateMap<Product, ProductBaseDTO>().ReverseMap();
        }
    }
}
