using AutoMapper;
using Domain.Entities;
using Application.Contracts.V1.Responses;

namespace WebApi.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<AttributeValue, AttributeValueResponse>();
            CreateMap<AttributeType, AttributeTypeResponse>();
            CreateMap<Item, ItemResponse>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<UserAddress, UserAddressResponse>();
            CreateMap<ApplicationUser, ApplicationUserResponse>();


        }
    }
}
