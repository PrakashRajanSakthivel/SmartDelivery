namespace PaymentService.Application.Contracts
{
    public class PaymentIntent
    {
        public string Id { get; set; }
        public string ClientSecret { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
    }
}
