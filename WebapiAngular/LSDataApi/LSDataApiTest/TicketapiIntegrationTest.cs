using LSDataApi;
using LSDataApi.Tests;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LSDataApiTest
{
    public class TicketapiIntegrationTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;

        public TicketapiIntegrationTest(TestFixture<Startup> fixture)

        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task GetApplicationMasterPassTest()
        {
            var response = await Client.GetAsync("api/Ticketapi");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetApplicationMastercountTest()
        {
            var response = await Client.GetAsync("api/ApplicationMasters");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}