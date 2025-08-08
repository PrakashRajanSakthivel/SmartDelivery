using System.Text.Json.Serialization;

namespace RestaurentService.Application.Restaurents.Commands.Category
{
    public class CreateCategoryRequest
    {
        [JsonPropertyName("restaurantId")]
        public Guid RestaurantId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("displayOrder")]
        public int DisplayOrder { get; set; }
    }
} 