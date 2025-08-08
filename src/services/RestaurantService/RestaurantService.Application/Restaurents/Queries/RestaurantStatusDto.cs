using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Queries
{
    public class RestaurantStatusDto
    {
        public Guid RestaurantId { get; set; }
        public RestaurantStatus Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? LastChangeReason { get; set; }
    }
}