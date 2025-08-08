using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurentService.Domain.Entites;
using RestaurentService.Domain.Interfaces;
using Shared.Data.Interfaces;

namespace RestaurantService.Domain.Interfaces
{
    public interface IRestaurantUnitOfWork : IUnitOfWork
    {
        IRestaurantRepository Restaurants { get; }
        DbSet<MenuItem> MenuItems { get; }
        DbSet<Category> Categories { get; }
    }
}
