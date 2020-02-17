using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DataApi.DBContext;
using System.Linq;
using DataApi.Models;
using System.Security.Principal;
using System.Security.Claims;
using log4net;

namespace DataApi.api
{
    public class BaseAPIController : ApiController
    {
        private static readonly ILog BLog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ///http://bitoftech.net/2015/03/11/asp-net-identity-2-1-roles-based-authorization-authentication-asp-net-web-api/
        /// http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
        /// http://www.dotnetmob.com/angular-5-tutorial/angular-5-login-and-logout-with-web-api-using-token-based-authentication/
        /// https://medium.com/aviabird/http-interceptor-angular2-way-e57dc2842462
        /// https://scotch.io/@kashyapmukkamala/using-http-interceptor-with-angular2
        /// https://www.youtube.com/watch?v=rbHSTJBhJ44
        /// https://medium.com/better-programming/creating-angular-webapp-for-multiple-views-and-screen-sizes-50fe8a83c433

        protected readonly TicketTrackerEntities2 TicketDB = new TicketTrackerEntities2();
        protected HttpResponseMessage ToJson(dynamic obj)
        {
            try
            {
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                return response;
            }
            catch(System.Exception e)
            {
                BLog.Error(e);
                throw;
            }
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