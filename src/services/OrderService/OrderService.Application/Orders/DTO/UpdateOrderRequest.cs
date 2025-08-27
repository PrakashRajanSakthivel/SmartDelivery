using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrderService.Application.Orders.DTO
{
    public class UpdateOrderRequest
    {
        [JsonPropertyName("orderId")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("items")]
        public List<UpdateOrderItemRequest> Items { get; set; } = new();

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }

    public class UpdateOrderItemRequest
    {
        [JsonPropertyName("orderItemId")]
        public Guid? OrderItemId { get; set; }

        [JsonPropertyName("menuItemId")]
        public Guid MenuItemId { get; set; }

        [JsonPropertyName("itemName")]
        public string ItemName { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }
    }
}