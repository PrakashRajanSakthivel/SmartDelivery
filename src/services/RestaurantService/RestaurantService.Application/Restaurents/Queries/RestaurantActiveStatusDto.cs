using RestaurentService.Domain.Entites;

namespace RestaurantService.Application.Restaurents.Queries
{
    public class RestaurantActiveStatusDto
    {
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public RestaurantStatus Status { get; set; }
        public bool HasAvailableMenuItems { get; set; }
        public bool IsAvailableForOrders { get; set; }
        public string? Reason { get; set; }
    }
}
