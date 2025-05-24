namespace OrderService.Domain.Entites
{
    public enum OrderStatus
    {
        PendingPayment,
        Paid,
        Preparing,
        Ready,
        Completed,
        Cancelled,
        Created,
        Delivered,
        Refunded,
        ReadyForDelivery,
        OutForDelivery,
        Returned
    }

}
