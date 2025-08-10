namespace RestaurantService.Application.Restaurents.Queries
{
    public class RestaurantOperatingHoursDto
    {
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string? OpeningHours { get; set; }
        public bool IsCurrentlyOpen { get; set; }
        public string? NextOpeningTime { get; set; }
        public string? NextClosingTime { get; set; }
    }
}
