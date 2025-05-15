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

        public async Task<bool> IsPresent(Guid Id)
        {
            var payload = new
            {
                RestaurantId = Id
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/GetRestaurant/", content);

            return response.IsSuccessStatusCode;
        }
    }
}
