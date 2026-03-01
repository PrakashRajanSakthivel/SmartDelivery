using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Automation.E2E
{
    public class OrderProcessingFlowTest
    {
        private readonly HttpClient _client = new HttpClient();
        private dynamic? _restaurantDetails;

        // Fixed GUID matching testuser's Id in HardcodedUserRepository - reused across all test runs
        private const string TestUserId = "00000000-0000-0000-0000-000000000002";

        // Cached token - fetched once per process lifetime, reused on reruns
        private static string? _cachedToken;
        private static readonly SemaphoreSlim _tokenLock = new SemaphoreSlim(1, 1);

        [Fact]
        public async Task CompleteOrderFlow_ReturnsOrderIdOrPaymentError()
        {
            // 1. Insert restaurant, dishes, prices (hardcoded)
            var restaurantId = await InsertRestaurant();
            var menuItemId = await GetMenuItemIdFromRestaurant(restaurantId);

            // 2. Authenticate user and get token (created once, reused across runs)
            var token = await GetOrFetchTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3. Add menu item to cart for the user
            await AddToCart(TestUserId, menuItemId, restaurantId);

            // 4. Create order and pay
            var (orderId, paymentError) = await CreateOrderAndPay(TestUserId, restaurantId, menuItemId);

            // 5. Assert result
            if (orderId != null)
            {
                Console.WriteLine($"Order placed successfully. Order ID: {orderId}");
                Assert.NotNull(orderId);
            }
            else
            {
                Console.WriteLine($"Payment failed: {paymentError}");
                Assert.Fail($"Order creation failed: {paymentError}");
            }
        }

        private async Task<Guid> InsertRestaurant()
        {
            // Use the correct schema for CreateRestaurantRequest
            var restaurantPayload = new
            {
                Name = "Testaurant",
                Description = "A test restaurant",
                Address = "123 Main St",
                PhoneNumber = "123-456-7890",
                DeliveryFee = 2.50m,
                MinOrderAmount = 10.00m,
                Categories = new[]
                {
                    new { Name = "Pizza", DisplayOrder = 1 }
                },
                MenuItems = new[]
                {
                    new {
                        Name = "Cheese Pizza",
                        Description = "Classic cheese pizza",
                        Price = 10.99m,
                        CategoryId = (Guid?)null,
                        IsVegetarian = true,
                        IsVegan = false,
                        PreparationTime = 15
                    }
                }
            };
            var response = await _client.PostAsync("http://localhost:5002/api/restaurants", new StringContent(
                JsonConvert.SerializeObject(restaurantPayload), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            // Try to get the ID from the Location header
            var location = response.Headers.Location?.ToString();
            if (!string.IsNullOrEmpty(location))
            {
                // Expecting .../api/restaurants/{id}
                var idStr = location.Split('/').Last();
                if (Guid.TryParse(idStr, out var id))
                    return id;
            }
            // Fallback: try to parse body if present
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
            {
                dynamic result = JsonConvert.DeserializeObject(content);
                if (result != null && result.id != null)
                    return result.id;
            }
            throw new Exception("Could not determine restaurant ID from response");
        }


        private async Task<Guid> GetMenuItemIdFromRestaurant(Guid restaurantId)
        {
            // Get restaurant details to fetch menu item ID
            var response = await _client.GetAsync($"http://localhost:5002/api/restaurants/{restaurantId}/details");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            _restaurantDetails = JsonConvert.DeserializeObject(content);
            if (_restaurantDetails != null && _restaurantDetails.menuItems != null && _restaurantDetails.menuItems.Count > 0)
                return _restaurantDetails.menuItems[0].id;
            throw new Exception("No menu items found for restaurant");
        }

        private async Task<string> GetOrFetchTokenAsync()
        {
            if (_cachedToken != null)
                return _cachedToken;

            await _tokenLock.WaitAsync();
            try
            {
                // Double-check after acquiring the lock
                if (_cachedToken != null)
                    return _cachedToken;

                _cachedToken = await GetUserToken();
                return _cachedToken;
            }
            finally
            {
                _tokenLock.Release();
            }
        }

        private async Task<string> GetUserToken()
        {
            var response = await _client.PostAsync("http://localhost:5001/api/auth/login", new StringContent(
                JsonConvert.SerializeObject(new { Username = "testuser", Password = "password" }), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);
            if (result != null && result.token != null)
                return result.token;
            throw new Exception("No token returned from auth service");
        }


        private async Task AddToCart(string userId, Guid menuItemId, Guid restaurantId)
        {
            // Use cached restaurant details instead of calling non-existent /api/menuitems endpoint
            if (_restaurantDetails == null)
            {
                var response = await _client.GetAsync($"http://localhost:5002/api/restaurants/{restaurantId}/details");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _restaurantDetails = JsonConvert.DeserializeObject(content);
            }

            dynamic? menuItem = null;
            foreach (var item in _restaurantDetails.menuItems)
            {
                if (item.id == menuItemId.ToString() || item.id == menuItemId)
                {
                    menuItem = item;
                    break;
                }
            }

            if (menuItem == null)
                throw new Exception("Menu item not found in restaurant details");

            var payload = new
            {
                MenuItemId = menuItemId.ToString(),
                MenuItemName = (string)menuItem.name,
                Quantity = 1,
                UnitPrice = (decimal)menuItem.price,
                ImageUrl = (string?)menuItem.imageUrl
            };
            var cartResponse = await _client.PostAsync($"http://localhost:5004/api/cart/{userId}/items", new StringContent(
                JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
            cartResponse.EnsureSuccessStatusCode();
        }


        private async Task<(Guid? orderId, string paymentError)> CreateOrderAndPay(string userId, Guid restaurantId, Guid menuItemId)
        {
            // Use cached restaurant details instead of calling non-existent /api/menuitems endpoint
            if (_restaurantDetails == null)
            {
                var detailsResponse = await _client.GetAsync($"http://localhost:5002/api/restaurants/{restaurantId}/details");
                detailsResponse.EnsureSuccessStatusCode();
                var detailsContent = await detailsResponse.Content.ReadAsStringAsync();
                _restaurantDetails = JsonConvert.DeserializeObject(detailsContent);
            }

            dynamic? menuItem = null;
            foreach (var item in _restaurantDetails.menuItems)
            {
                if (item.id == menuItemId.ToString() || item.id == menuItemId)
                {
                    menuItem = item;
                    break;
                }
            }

            if (menuItem == null)
                throw new Exception("Menu item not found in restaurant details");

            var orderPayload = new
            {
                UserId = userId,
                RestaurantId = restaurantId,
                Items = new[]
                {
                    new {
                        MenuItemId = menuItemId,
                        ItemName = (string)menuItem.name,
                        Quantity = 1,
                        UnitPrice = (decimal)menuItem.price
                    }
                },
                Notes = "Test order"
            };
            var response = await _client.PostAsync("http://localhost:5003/api/orders", new StringContent(
                JsonConvert.SerializeObject(orderPayload), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                // Try to get the order ID from the response body or headers
                if (!string.IsNullOrWhiteSpace(content))
                {
                    dynamic result = JsonConvert.DeserializeObject(content);
                    if (result != null && result.id != null)
                        return (result.id, null);
                }
                // Try Location header as fallback
                var location = response.Headers.Location?.ToString();
                if (!string.IsNullOrEmpty(location))
                {
                    var idStr = location.Split('/').Last();
                    if (Guid.TryParse(idStr, out var id))
                        return (id, null);
                }
                return (null, "Order created but ID not found in response");
            }
            else
            {
                return (null, content);
            }
        }
    }
}
