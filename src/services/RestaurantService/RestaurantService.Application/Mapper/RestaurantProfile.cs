using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantService.Application.Restaurents.Queries;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Entites;

namespace RestaurantService.Application.Mapper
{
    // AutoMapper Profile
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            // Restaurant mappings
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Restaurant, RestaurantDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Restaurant, RestaurantStatusDto>()
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.LastChangeReason, opt => opt.Ignore()); // Set to null for now

            // Menu Item mappings
            //CreateMap<MenuItem, Restaurents.Queries.MenuItemDto>()
            //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            //// Category mappings
            //CreateMap<Category, CategoryDto>()
            //    .ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

            // Restaurant Menu mapping
            CreateMap<Restaurant, RestaurantMenuDto>()
                .ForMember(dest => dest.RestaurantId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.UncategorizedItems, opt => opt.MapFrom(src => src.MenuItems.Where(m => m.CategoryId == null)));
        }
    }
}
