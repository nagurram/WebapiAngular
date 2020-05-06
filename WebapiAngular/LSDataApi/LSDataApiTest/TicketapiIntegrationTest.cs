using LSDataApi;
using LSDataApi.Controllers;
using LSDataApi.Tests;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    }
}