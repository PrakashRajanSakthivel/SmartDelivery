using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Entites;

namespace RestaurantService.Application.Mapper
{
    // AutoMapper Profile
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<Restaurant, RestaurantDto>();
            // Add other custom mappings if needed
        }
    }
}
