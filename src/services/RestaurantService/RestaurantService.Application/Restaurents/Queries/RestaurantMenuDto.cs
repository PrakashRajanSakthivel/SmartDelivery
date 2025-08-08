using RestaurantService.Application.Restaurents.Queries;

namespace RestaurentService.Application.Restaurents.Queries
{
    public class RestaurantMenuDto
    {
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<CategoryDto> Categories { get; set; } = new();
        public List<MenuItemDto> UncategorizedItems { get; set; } = new();
    }
} 