using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Filters101.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace IntegrationTests.AuthorsController
{
    public class Delete : AuthorsControllerTestBase
    {
        private readonly HttpClient _client;

        public Delete()
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsNotFoundForId0(string controllerName)
        {
            var response = await _client.DeleteAsync($"/api/{controllerName}/0");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal("0", stringResponse);
        }

        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsOkGivenValidAuthorData(string controllerName)
        {
            var response = await _client.GetAsync($"/api/{controllerName}");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var authors = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringResponse).ToList();
            var idToDelete = authors.FirstOrDefault().Id;
            var response2 = await _client.DeleteAsync($"/api/{controllerName}/{idToDelete}");
            response2.EnsureSuccessStatusCode();
        }
    }
}
