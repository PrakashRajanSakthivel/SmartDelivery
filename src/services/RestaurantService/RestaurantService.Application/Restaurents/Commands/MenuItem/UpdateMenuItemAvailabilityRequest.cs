using System.Text.Json.Serialization;

namespace RestaurentService.Application.Restaurents.Commands.MenuItem
{
    public class UpdateMenuItemAvailabilityRequest
    {
        [JsonPropertyName("menuItemId")]
        public Guid MenuItemId { get; set; }

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }
    }
} 