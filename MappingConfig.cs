using AutoMapper;
using DotNet_MinimalAPI_Example.Models;
using DotNet_MinimalAPI_Example.Models.DTO;

namespace DotNet_MinimalAPI_Example
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
