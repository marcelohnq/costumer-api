using Newtonsoft.Json;
using ProvaPub.Models;
using ProvaPub.Services.Payment;

namespace ProvaPub.Tests.Controllers
{
    public class Parte3ControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private const string RouteOrders = "/Parte3/orders";
        private const string CustomerId = "1";

        private readonly HttpClient _client;

        public Parte3ControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(PaymentMethod.Pix, 1, 1)]
        [InlineData(PaymentMethod.Creditcard, 1, 1.15)]
        [InlineData(PaymentMethod.Paypal, 1, 1.1)]
        public async Task PostOrder(PaymentMethod pm, decimal payment, decimal expected)
        {
            Order? Order = await PostJson(RouteOrders, pm, payment);

            Assert.NotNull(Order);
            Assert.Equal(expected, Order.Value);
            Assert.Equal(CustomerId, Order.CustomerId.ToString());
            Assert.Equal(DateTime.Now.ToShortDateString(), Order.OrderDate.ToShortDateString());
        }

        private async Task<Order?> PostJson(string url, PaymentMethod pm, decimal payment)
        {
            string request = $"{url}?paymentMethod={pm}&paymentValue={payment}&customerId={CustomerId}";
            HttpResponseMessage response = await _client.PostAsync(request, null);

            // Check if HttpStatus is 200
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(json);
        }
    }
}
