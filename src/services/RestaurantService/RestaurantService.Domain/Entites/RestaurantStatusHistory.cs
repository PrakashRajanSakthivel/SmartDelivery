namespace RestaurentService.Domain.Entites
{
    public class RestaurantStatusHistory
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public RestaurantStatus Status { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? ChangedBy { get; set; } // User/system/admin
        public string? Reason { get; set; }

        public Restaurant? Restaurant { get; set; }
    }
}