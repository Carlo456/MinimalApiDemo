using AutoMapper;
using MinimalApiDemo.Models;
using MinimalApiDemo.Models.DTO;

namespace MinimalApiDemo
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
