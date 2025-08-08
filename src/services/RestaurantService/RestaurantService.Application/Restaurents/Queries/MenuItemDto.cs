using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Queries
{
    public class MenuItemDto
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public string? ImageUrl { get; set; }
        public int PreparationTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CategoryName { get; set; }
    }
} 