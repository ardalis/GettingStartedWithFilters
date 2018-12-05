using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Filters101.Models;
using Newtonsoft.Json;
using System.Text;
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

        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsBadRequestGivenFullNameOver50Chars(string controllerName)
        {
            var authorToPost = new Author() { FullName = "12345678901234567890123456789012345678901234567890a", TwitterAlias = "test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(authorToPost), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/{controllerName}", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("FullName", stringResponse);
            Assert.Contains("The field FullName must be a string or array type with a maximum length of '50'.", stringResponse);
        }

        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsBadRequestGivenTwitterAliasOver30Chars(string controllerName)
        {
            var authorToPost = new Author() { FullName = "test", TwitterAlias = "123456789012345678901234567890a" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(authorToPost), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/{controllerName}", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("TwitterAlias", stringResponse);
            Assert.Contains("The field TwitterAlias must be a string or array type with a maximum length of '30'.", stringResponse);
        }

        [Theory]
        [InlineData("authors")]
        [InlineData("authors2")]
        public async Task ReturnsOkGivenValidAuthorData(string controllerName)
        {
            var authorToPost = new Author() { FullName = "John Doe", TwitterAlias = "johndoe" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(authorToPost), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/{controllerName}", jsonContent);
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var author = JsonConvert.DeserializeObject<Author>(stringResponse);

            Assert.Equal("John Doe", author.FullName);
            Assert.True(author.Id > 0);
        }
    }
}
