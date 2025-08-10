using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace OrderService.Application.Common
{
    public class RestaurentService : IRestaurentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RestaurentService> _logger;

        public RestaurentService(HttpClient httpClient, ILogger<RestaurentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<RestaurantValidationDto?> GetRestaurantForValidationAsync(Guid restaurantId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/restaurants/{restaurantId}/validation");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to get restaurant validation data: {RestaurantId}, Status: {StatusCode}", 
                        restaurantId, response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<RestaurantValidationDto>>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting restaurant validation data: {RestaurantId}", restaurantId);
                return null;
            }
        }
    }

    // Supporting DTOs for API responses
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
    }
}
