using AutoMapper;
using Domain.Entities;
using Application.Contracts.V1.Responses;

namespace WebApi.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Item, ItemResponse>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<AttributeType, AttributeTypeResponse>();
            CreateMap<UserAddress, UserAddressResponse>();
            CreateMap<Cart, CartResponse>();
        }
    }
}
