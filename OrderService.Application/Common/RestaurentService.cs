using System.Text.Json;
using System.Text;

namespace OrderService.Application.Common
{
    public class RestaurentService : IRestaurentService
    {
        private readonly HttpClient _httpClient;

        public RestaurentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ProcessPaymentAsync(string orderId, decimal amount)
        {
            var payload = new
            {
                OrderId = orderId,
                Amount = amount
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/payment/process", content);

            return response.IsSuccessStatusCode;
        }
    }
}
