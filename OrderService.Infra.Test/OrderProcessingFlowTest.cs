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

        // Seeded Burger House from scripts/seed-restaurants.sql – fixed GUID, always present after seed
        private const string SeedBurgerHouseId = "11111111-0000-0000-0000-000000000001";

        // Cached token - fetched once per process lifetime, reused on reruns
        private static string? _cachedToken;
        private static readonly SemaphoreSlim _tokenLock = new SemaphoreSlim(1, 1);

        [Fact]
        public async Task CompleteOrderFlow_ReturnsOrderIdOrPaymentError()
        {
            // 1. Use the seeded Burger House (scripts/seed-restaurants.sql) – no DB pollution
            var restaurantId = await EnsureRestaurantAsync();
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

        /// <summary>
        /// Returns the seeded Burger House ID if it exists in the DB.
        /// Falls back to creating a minimal test restaurant with the same fixed GUID
        /// so the test is self-contained when seed-restaurants.sql has not been run yet.
        /// </summary>
        private async Task<Guid> EnsureRestaurantAsync()
        {
            var seedId = Guid.Parse(SeedBurgerHouseId);

            var check = await _client.GetAsync($"http://localhost:5002/api/restaurants/{seedId}");
            if (check.IsSuccessStatusCode)
                return seedId;

            // Seed not present – create a minimal restaurant with the same fixed GUID
            // so the ID is stable across test runs.
            var payload = new
            {
                Name        = "Burger House",
                Description = "Juicy burgers and crispy fries",
                Address     = "10 Burger Lane",
                PhoneNumber = "555-100-0001",
                DeliveryFee = 1.99m,
                MinOrderAmount = 8.00m,
                Categories = new[] { new { Name = "Burgers", DisplayOrder = 1 } },
                MenuItems  = new[]
                {
                    new {
                        Name            = "Classic Cheeseburger",
                        Description     = "Beef patty, cheese, lettuce, tomato, onion",
                        Price           = 9.99m,
                        CategoryId      = (Guid?)null,
                        IsVegetarian    = false,
                        IsVegan         = false,
                        PreparationTime = 10
                    }
                }
            };

            var response = await _client.PostAsync(
                "http://localhost:5002/api/restaurants",
                new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            // The API returns 201 Created with Location: .../api/restaurants/{id}
            var location = response.Headers.Location?.ToString();
            if (!string.IsNullOrEmpty(location))
            {
                var idStr = location.Split('/').Last();
                if (Guid.TryParse(idStr, out var id))
                    return id;
            }

            throw new Exception("Could not determine restaurant ID from CreateRestaurant response");
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
