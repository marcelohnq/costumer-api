using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProvaPub.Models;

namespace ProvaPub.Tests
{
    public class Parte2ControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string RouteProducts = "/Parte2/products";
        private const string RouteCustomers = "/Parte2/customers";

        private readonly HttpClient _client;

        public Parte2ControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts()
        {
            EntityList<Product>? list = await GetJson<Product>(RouteProducts);

            Assert.NotNull(list);
            Assert.Equal(10, list.Itens.Count);
            Assert.True(list.HasNext);
        }

        [Fact]
        public async Task GetCustomers()
        {
            EntityList<Customer>? list = await GetJson<Customer>(RouteCustomers);

            Assert.NotNull(list);
            Assert.Equal(10, list.Itens.Count);
            Assert.True(list.HasNext);
        }

        [Fact]
        public async Task Get_Products_Pagination()
        {
            EntityList<Product>? page1 = await GetJson<Product>($"{RouteProducts}?page=1");
            EntityList<Product>? page2 = await GetJson<Product>($"{RouteProducts}?page=2");

            Assert.NotNull(page1);
            Assert.NotNull(page2);
            Assert.Equal(10, page1.Itens.Count);
            Assert.Equal(10, page2.Itens.Count);
            Assert.True(page1.HasNext);
            Assert.False(page2.HasNext);
            Assert.Equal(1, page1.Itens.FirstOrDefault()?.Id);
            Assert.Equal(10, page1.Itens.LastOrDefault()?.Id);
            Assert.Equal(11, page2.Itens.FirstOrDefault()?.Id);
            Assert.Equal(20, page2.Itens.LastOrDefault()?.Id);
        }

        [Fact]
        public async Task Get_Customers_Pagination()
        {
            EntityList<Customer>? page1 = await GetJson<Customer>($"{RouteCustomers}?page=1");
            EntityList<Customer>? page2 = await GetJson<Customer>($"{RouteCustomers}?page=2");

            Assert.NotNull(page1);
            Assert.NotNull(page2);
            Assert.Equal(10, page1.Itens.Count);
            Assert.Equal(10, page2.Itens.Count);
            Assert.True(page1.HasNext);
            Assert.False(page2.HasNext);
            Assert.Equal(1, page1.Itens.FirstOrDefault()?.Id);
            Assert.Equal(10, page1.Itens.LastOrDefault()?.Id);
            Assert.Equal(11, page2.Itens.FirstOrDefault()?.Id);
            Assert.Equal(20, page2.Itens.LastOrDefault()?.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-100)]
        public async Task Get_Products_Page_Border(int pageBorder)
        {
            EntityList<Product>? list = await GetJson<Product>($"{RouteProducts}?page={pageBorder}");

            Assert.NotNull(list);
            Assert.Equal(10, list.Itens.Count);
            Assert.True(list.HasNext);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-100)]
        public async Task Get_Customers_Page_Border(int pageBorder)
        {
            EntityList<Customer>? list = await GetJson<Customer>($"{RouteCustomers}?page={pageBorder}");

            Assert.NotNull(list);
            Assert.Equal(10, list.Itens.Count);
            Assert.True(list.HasNext);
        }

        [Fact]
        public async Task Get_Products_EndPagination()
        {
            bool next = true;
            int page = 1;
            EntityList<Product>? list = null;

            while (next == true)
            {
                list = await GetJson<Product>($"{RouteProducts}?page={page}");
                next = list?.HasNext ?? false;
                page++;
            }

            Assert.NotNull(list);
            Assert.Equal(10, list?.Itens.Count ?? -1);
            Assert.False(list?.HasNext ?? true);

            // Get page overflow
            list = await GetJson<Product>($"{RouteProducts}?page={page}");

            Assert.NotNull(list);
            Assert.Equal(0, list?.Itens.Count ?? -1);
            Assert.False(list?.HasNext ?? true);
        }

        [Fact]
        public async Task Get_Customers_EndPagination()
        {
            bool next = true;
            int page = 1;
            EntityList<Customer>? list = null;

            while (next == true)
            {
                list = await GetJson<Customer>($"{RouteCustomers}?page={page}");
                next = list?.HasNext ?? false;
                page++;
            }

            Assert.NotNull(list);
            Assert.Equal(10, list?.Itens.Count ?? -1);
            Assert.False(list?.HasNext ?? true);

            // Get page overflow
            list = await GetJson<Customer>($"{RouteCustomers}?page={page}");

            Assert.NotNull(list);
            Assert.Equal(0, list?.Itens.Count ?? -1);
            Assert.False(list?.HasNext ?? true);
        }

        private async Task<EntityList<T>?> GetJson<T>(string request)
        {
            HttpResponseMessage response = await _client.GetAsync(request);

            // Check if HttpStatus is 200
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EntityList<T>>(json);
        }
    }
}
