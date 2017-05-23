using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Filters101.Models;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests.AuthorsController
{
    public class GetById : AuthorsControllerTestBase
    {
        private readonly HttpClient _client;

        public GetById()
        {
            _client = base.GetClient();
        }

        [Fact]
        public async Task ReturnsNotFoundForId0SingleTest()
        {
            var response = await _client.GetAsync("/api/authors/0");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal("0", stringResponse);
        }


        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsSteveForId1(string controllerName)
        {
            var response = await _client.GetAsync($"/api/{controllerName}/1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Author>(stringResponse);

            Assert.Equal("Steve Smith", result.FullName);
        }

        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsNotFoundForId0(string controllerName)
        {
            var response = await _client.GetAsync($"/api/{controllerName}/0");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal("0", stringResponse);
        }
    }
}
