using AutoMapper;
using Domain.Entities;
using Web.Contracts.V1.Responses;

namespace Web.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<AttributeValue, AttributeValueResponse>();
            CreateMap<AttributeType, AttributeTypeResponse>();
            CreateMap<Item, ItemResponse>();
            CreateMap<Item, OrderItemResponse>();

            CreateMap<Category, CategoryResponse>();
            CreateMap<UserAddress, UserAddressResponse>();
            CreateMap<ApplicationUser, ApplicationUserResponse>();
            CreateMap<Order, OrderResponse>();

        }
    }
}
