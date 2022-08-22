using AutoMapper;
using FirstApi.Data.Entities;
using FirstApi.Dtos.ProductDtos;

namespace FirstApi.MapProfiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductReturnDto>();
               
        }
    }
}
