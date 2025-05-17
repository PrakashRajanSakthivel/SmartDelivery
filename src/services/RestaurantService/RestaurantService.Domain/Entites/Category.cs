using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurentService.Domain.Entites
{
    public class Category
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }  // Important for restaurant-specific categories
        public string Name { get; set; } = default!;
        public int DisplayOrder { get; set; }  // Helps control how categories appear in UI

        public Restaurant Restaurant { get; set; } = default!;
        public ICollection<MenuItem> MenuItems { get; set; } = [];
    }

}
