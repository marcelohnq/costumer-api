using Microsoft.AspNetCore.Mvc.Testing;

namespace ProvaPub.Tests
{
    public class Parte1ControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string Route = "/Parte1";

        private readonly HttpClient _client;

        public Parte1ControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get()
        {
            HttpResponseMessage response = await _client.GetAsync(Route);

            // Check if HttpStatus is 200
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            Assert.True(int.TryParse(result, out int _));
        }

        [Fact]
        public async Task Get_DiffNum()
        {
            HttpResponseMessage response = await _client.GetAsync(Route);
            string result1 = await response.Content.ReadAsStringAsync();

            response = await _client.GetAsync(Route);
            string result2 = await response.Content.ReadAsStringAsync();

            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public async Task Get_Stress()
        {
            HttpResponseMessage response = await _client.GetAsync(Route);
            string result1 = await response.Content.ReadAsStringAsync();

            for (int i=0; i<100; i++)
            {
                response = await _client.GetAsync(Route);
                string result2 = await response.Content.ReadAsStringAsync();

                Assert.NotEqual(result1, result2);

                result1 = result2;
            }
        }
    }
}