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
    public class TicketapiIntegrationTest// : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;
        /*
        public TicketapiIntegrationTest(TestFixture<Startup> fixture)

        {
            Client = fixture.Client;
        }
*/

        public TicketapiIntegrationTest()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost/webapp");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            JObject obj = JObject.FromObject(new
            {
                username = "naren.7090@gmail.com",
                password = "1234"
            });

            var body = obj.ToString();
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
            var responsetoken = Client.PostAsync("api/userapi/authenticate", content).Result;
            string authtoken = "";
            if (responsetoken != null)
            {
                var jsonString = responsetoken.Content.ReadAsStringAsync().Result;
                JObject objrespose = JObject.Parse(jsonString);
                authtoken = (string)objrespose["Token"];
            }
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
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