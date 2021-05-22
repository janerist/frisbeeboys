using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Frisbeeboys.Api.Tests.Helpers
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task AssertOk(this HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.True(false, $"Expected 200 OK, got {(int)response.StatusCode}. Response body:\n\n{content}");
            }
        }
        
        public static async Task AssertBadRequest(this HttpResponseMessage response, string expectedMessage = null)
        {
            var content = await response.Content.ReadAsStringAsync();
            
            if (response.StatusCode != HttpStatusCode.BadRequest)
            {
                Assert.True(false, $"Expected 400 Bad Request, got {(int)response.StatusCode}. Response body:\n\n{content}");
            }
            else if (expectedMessage != null)
            {
                var message = JsonSerializer.Deserialize<JsonElement>(content).GetProperty("message").GetString();
                Assert.Equal(expectedMessage, message);
            }
        }
    }
}