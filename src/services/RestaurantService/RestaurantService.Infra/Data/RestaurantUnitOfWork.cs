﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Domain.Interfaces;
using RestaurentService.Infra.Data;
using Shared.Data.Repositories;

namespace RestaurantService.Infra.Data
{
    public class RestaurantUnitOfWork : UnitOfWork<RestaurantDbContext>, IRestaurantUnitOfWork
    {
        public IRestaurantRepository Restaurants { get; }

        public RestaurantUnitOfWork(
            RestaurantDbContext context,
            IRestaurantRepository restaurants) : base(context)
        {
            Restaurants = restaurants;
        }
    }
}
