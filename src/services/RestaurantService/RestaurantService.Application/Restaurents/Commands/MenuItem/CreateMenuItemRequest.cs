using System.Text.Json.Serialization;

namespace RestaurentService.Application.Restaurents.Commands.MenuItem
{
    public class CreateMenuItemRequest
    {
        [JsonPropertyName("restaurantId")]
        public Guid RestaurantId { get; set; }

        [JsonPropertyName("categoryId")]
        public Guid? CategoryId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("isVegetarian")]
        public bool IsVegetarian { get; set; }

        [JsonPropertyName("isVegan")]
        public bool IsVegan { get; set; }

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("preparationTime")]
        public int PreparationTime { get; set; } = 15;

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; } = true;
    }
} 