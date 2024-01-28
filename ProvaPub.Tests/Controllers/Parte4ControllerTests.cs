using ProvaPub.Services.Payment;

namespace ProvaPub.Tests.Controllers
{
    public class Parte4ControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private const string Route = "/Parte4/CanPurchase";
        private const string RouteOrders = "/Parte3/orders";

        private readonly HttpClient _client;

        public Parte4ControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_FirstPurchase_True()
        {
            Assert.True(await GetCanPurchase(Route, 50, 1));
        }

        [Fact]
        public async Task Get_FirstPurchase_False()
        {
            Assert.False(await GetCanPurchase(Route, 5555, 1));
        }

        [Fact]
        public async Task Get_OnlySingleTimeMonth()
        {
            await PostOrder(RouteOrders, PaymentMethod.Pix, 50, 2);
            Assert.False(await GetCanPurchase(Route, 1, 2));
        }

        private async Task<bool> GetCanPurchase(string url, decimal value, int customerId)
        {
            string request = $"{url}?customerId={customerId}&purchaseValue={value}";
            HttpResponseMessage response = await _client.GetAsync(request);

            // Check if HttpStatus is 200
            response.EnsureSuccessStatusCode();

            string s = await response.Content.ReadAsStringAsync();
            _ = bool.TryParse(s, out bool result);
            return result;
        }

        private async Task PostOrder(string url, PaymentMethod pm, decimal payment, int customerId)
        {
            string request = $"{url}?paymentMethod={pm}&paymentValue={payment}&customerId={customerId}";
            HttpResponseMessage response = await _client.PostAsync(request, null);

            // Check if HttpStatus is 200
            response.EnsureSuccessStatusCode();
        }
    }
}
