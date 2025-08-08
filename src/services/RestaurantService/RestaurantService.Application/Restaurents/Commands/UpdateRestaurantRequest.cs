using System.Text.Json.Serialization;

namespace RestaurentService.Application.Restaurents.Commands
{
    public class UpdateRestaurantRequest
    {
        [JsonPropertyName("restaurantId")]
        public Guid RestaurantId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("openingHours")]
        public string? OpeningHours { get; set; }

        [JsonPropertyName("cuisineType")]
        public string? CuisineType { get; set; }
    }
}