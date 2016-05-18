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
    public class Post : AuthorsControllerTestBase
    {
        private readonly HttpClient _client;

        public Post()
        {
            _client = base.GetClient();
        }

        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsBadRequestGivenNoAuthorName(string controllerName)
        {
            var authorToPost = new Author() {Id=10, FullName = "", TwitterAlias = "test"};
            var jsonContent = new StringContent(JsonConvert.SerializeObject(authorToPost), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/{controllerName}", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("FullName", stringResponse);
            Assert.Contains("The FullName field is required.", stringResponse);
        }
    }
}
