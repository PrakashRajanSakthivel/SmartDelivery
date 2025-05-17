using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurentService.Domain.Entites
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid? CategoryId { get; set; }

        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public bool IsVegetarian { get; set; }  // Quick filter
        public bool IsVegan { get; set; }       // Quick filter
        public string? ImageUrl { get; set; }   // Food images increase conversions
        public int PreparationTime { get; set; } // In minutes - helps set delivery expectations

        public Restaurant Restaurant { get; set; } = default!;
        public Category? Category { get; set; }
    }

}
