using RestaurentService.Domain.Entites;

namespace RestaurantService.Application.Restaurents.Queries
{
    public class RestaurantValidationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public RestaurantStatus Status { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public string? OpeningHours { get; set; }
        public List<MenuItemValidationDto> MenuItems { get; set; } = new List<MenuItemValidationDto>();
    }

    public class MenuItemValidationDto
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
