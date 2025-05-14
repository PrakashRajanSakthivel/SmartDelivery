using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurentService.Domain.Entites
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? LogoUrl { get; set; }
        public string? Address { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal MinOrderAmount { get; set; }
        public double AverageRating { get; set; }
        public int EstimatedDeliveryTime { get; set; } // In minutes

        public ICollection<MenuItem> MenuItems { get; set; } = [];
        public ICollection<Category> Categories { get; set; } = [];
        public string PhoneNumber { get; set; }
    }

}
