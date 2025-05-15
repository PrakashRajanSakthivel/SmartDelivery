using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurentService.Domain.Interfaces;
using Shared.Data.Interfaces;

namespace RestaurantService.Domain.Interfaces
{
    public interface IRestaurantUnitOfWork : IUnitOfWork
    {
        IRestaurantRepository Restaurants { get; }
    }
}
