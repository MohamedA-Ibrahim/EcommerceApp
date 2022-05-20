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
            CreateMap<Item, ItemResponse>()
                     .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.ApplicationUser));

            CreateMap<Category, CategoryResponse>();
            CreateMap<UserAddress, UserAddressResponse>();
            CreateMap<ApplicationUser, ApplicationUserResponse>();
            CreateMap<Order, OrderResponse>()
                      .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name));
            ;
        }
    }
}
