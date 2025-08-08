using System.Text.Json.Serialization;
using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Commands
{
    public class UpdateRestaurantStatusRequest
    {
        [JsonPropertyName("restaurantId")]
        public Guid RestaurantId { get; set; }

        [JsonPropertyName("newStatus")]
        public RestaurantStatus NewStatus { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}