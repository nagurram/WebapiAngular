using LSDataApi;
using LSDataApi.Tests;
using Newtonsoft.Json.Linq;
using System.Net.Http;
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

        #region "Get Methods"

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

        [Fact]
        public async Task AppMasterTest()
        {
            var response = await Client.GetAsync("api/Ticketapi/AppMaster");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RootcauseMasterTest()
        {
            var response = await Client.GetAsync("api/Ticketapi/RootcauseMaster");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModuleMasterTest()
        {
            var response = await Client.GetAsync("api/Ticketapi/ModuleMaster");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PriorityMasterTest()
        {
            var response = await Client.GetAsync("api/Ticketapi/PriorityMaster");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task StatusMasterTest()
        {
            var response = await Client.GetAsync("api/Ticketapi/StatusMaster");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UserMasterTest()
        {
            var response = await Client.GetAsync("api/Ticketapi/UserMaster");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TypeMasterTest()
        {
            var response = await Client.GetAsync("api/Ticketapi/TypeMaster");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AdduserTest()
        {
            JObject obj = JObject.FromObject(new
            {
                FirstName = "First Name Test",
                LastName = "Last Name Test",
                EmailId = "Firstname.LastName@testMail.com",
                Roleid = "1"
            });
            var body = obj.ToString();
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/adminapi/adduser", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        #endregion "Get Methods"
    }
}