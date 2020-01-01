using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ang4api.DBContext;
using System.Linq;
using ang4api.Models;
using System.Security.Principal;
using System.Security.Claims;
using log4net;

namespace ang4api.api
{
    public class BaseAPIController : ApiController
    {

        ///http://bitoftech.net/2015/03/11/asp-net-identity-2-1-roles-based-authorization-authentication-asp-net-web-api/
        /// http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
        /// http://www.dotnetmob.com/angular-5-tutorial/angular-5-login-and-logout-with-web-api-using-token-based-authentication/
        /// https://medium.com/aviabird/http-interceptor-angular2-way-e57dc2842462
        /// https://scotch.io/@kashyapmukkamala/using-http-interceptor-with-angular2
        /// https://www.youtube.com/watch?v=rbHSTJBhJ44

        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly TicketTrackerEntities TicketDB = new TicketTrackerEntities();
        protected HttpResponseMessage ToJson(dynamic obj)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            return response;
        }


        protected HttpResponseMessage BAppMaster()
        {
            return ToJson(TicketDB.ApplicationMasters.Select(p => new keyvalueModel { Id = p.ApplicationId, keyValue = p.ApplicationName, IsDeleted = p.IsDeleted }).AsEnumerable());
        }


        protected HttpResponseMessage BUserMaster()
        {
            return ToJson(TicketDB.UserMasters.Select(p => new keyvalueModel { Id = p.UserId, keyValue = p.FName + ", " + p.LName, IsDeleted = p.IsDeleted }).AsEnumerable());
        }


        protected HttpResponseMessage BStatusMaster()
        {
            return ToJson(TicketDB.StatusMasters.Select(p => new keyvalueModel { Id = p.StatusId, keyValue = p.StatusDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }


        protected HttpResponseMessage BPriorityMaster()
        {
            return ToJson(TicketDB.PriorityMasters.Select(p => new keyvalueModel { Id = p.PriorityId, keyValue = p.PriorityDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }


        protected HttpResponseMessage BModuleMaster()
        {
            return ToJson(TicketDB.ModuleMasters.Select(p => new keyvalueModel { Id = p.ModuleId, keyValue = p.ModuleName, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        protected HttpResponseMessage BTypeMaster()
        {
            return ToJson(TicketDB.TypeMasters.Select(p => new keyvalueModel { Id = p.TypeId, keyValue = p.TypeDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        protected HttpResponseMessage BRootCauseMaster()
        {
            return ToJson(TicketDB.RootCauseMasters.Select(p => new keyvalueModel { Id = p.RootCauseId, keyValue = p.Description, IsDeleted = p.Isdelete }).AsEnumerable());
        }

        protected  string GetClaimValue(string name)
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity == null ? null : claimsIdentity?.FindFirst(name);
            return claim == null ? null : claim.Value;
        }

    }
}