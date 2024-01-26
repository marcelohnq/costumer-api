using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProvaPub.Models;
using ProvaPub.Services.Payment;

namespace ProvaPub.Tests
{
    public class Parte3ControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string RouteOrders = "/Parte3/orders";
        private const string CustomerId = "1";

        private readonly HttpClient _client;

        public Parte3ControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(PaymentMethod.Pix, 1, 1)]
        [InlineData(PaymentMethod.Creditcard, 1, 1.15)]
        [InlineData(PaymentMethod.Paypal, 1, 1.1)]
        public async Task GetOrder(PaymentMethod pm, decimal payment, decimal expected)
        {
            Order? Order = await GetJson<Order>(RouteOrders, pm, payment);

            Assert.NotNull(Order);
            Assert.Equal(expected, Order.Value);
            Assert.Equal(CustomerId, Order.CustomerId.ToString());
            Assert.Equal(DateTime.Now.ToShortDateString(), Order.OrderDate.ToShortDateString());
        }

        private async Task<Order?> GetJson<T>(string url, PaymentMethod pm, decimal payment)
        {
            string request = $"{url}?paymentMethod={pm}&paymentValue={payment}&customerId={CustomerId}";
            HttpResponseMessage response = await _client.GetAsync(request);

            // Check if HttpStatus is 200
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(json);
        }
    }
}
