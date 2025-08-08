using AutoMapper;
using CartService.Domain;
using CartService.Application;

namespace CartService.Application.Mapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            // Cart to CartDto mapping
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems));

            // CartItem to CartItemDto mapping
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
        }
    }
} 