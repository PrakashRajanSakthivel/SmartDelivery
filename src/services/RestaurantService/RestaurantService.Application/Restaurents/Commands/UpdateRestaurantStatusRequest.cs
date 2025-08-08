using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Commands
{
    public class UpdateRestaurantStatusRequest
    {
        public Guid RestaurantId { get; set; }
        public RestaurantStatus NewStatus { get; set; }
        public string? Reason { get; set; }
    }
}