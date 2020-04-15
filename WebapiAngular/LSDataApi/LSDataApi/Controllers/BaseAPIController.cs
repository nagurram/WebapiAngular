using LsDataApi.Common;
using LSDataApi.DBContext;
using LSDataApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LSDataApi.api
{
    public class BaseAPIController : ControllerBase
    {
        ///http://bitoftech.net/2015/03/11/asp-net-identity-2-1-roles-based-authorization-authentication-asp-net-web-api/
        /// http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
        /// http://www.dotnetmob.com/angular-5-tutorial/angular-5-login-and-logout-with-web-api-using-token-based-authentication/
        /// https://medium.com/aviabird/http-interceptor-angular2-way-e57dc2842462
        /// https://scotch.io/@kashyapmukkamala/using-http-interceptor-with-angular2
        /// https://www.youtube.com/watch?v=rbHSTJBhJ44
        /// https://medium.com/better-programming/creating-angular-webapp-for-multiple-views-and-screen-sizes-50fe8a83c433

        protected readonly TicketTrackerContext TicketDB = new TicketTrackerContext();
        /*
        protected async Task<IActionResult> ToJson(dynamic obj)
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
        */

        [ProducesResponseType(StatusCodes.Status200OK)]
        protected async Task<IActionResult> BAppMaster()
        {
            return Ok(TicketDB.ApplicationMaster.Select(p => new keyvalueModel { Id = p.ApplicationId, keyValue = p.ApplicationName, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        protected async Task<IActionResult> BUserMaster()
        {
            return Ok(TicketDB.UserMaster.Select(p => new keyvalueModel { Id = p.UserId, keyValue = p.Fname + ", " + p.Lname, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        protected async Task<IActionResult> BStatusMaster()
        {
            return Ok(TicketDB.StatusMaster.Select(p => new keyvalueModel { Id = p.StatusId, keyValue = p.StatusDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        protected async Task<IActionResult> BPriorityMaster()
        {
            return Ok(TicketDB.PriorityMaster.Select(p => new keyvalueModel { Id = p.PriorityId, keyValue = p.PriorityDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        protected async Task<IActionResult> BModuleMaster()
        {
            return Ok(TicketDB.ModuleMaster.Select(p => new keyvalueModel { Id = p.ModuleId, keyValue = p.ModuleName, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        protected async Task<IActionResult> BTypeMaster()
        {
            return Ok(TicketDB.TypeMaster.Select(p => new keyvalueModel { Id = p.TypeId, keyValue = p.TypeDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        protected async Task<IActionResult> BRootCauseMaster()
        {
            return Ok(TicketDB.RootCauseMaster.Select(p => new keyvalueModel { Id = p.RootCauseId, keyValue = p.Description, IsDeleted = p.Isdelete }).AsEnumerable());
        }

        protected string GetClaimValue(string name)
        {
            if (name == Constants.UserId)
            {
                name = ClaimTypes.Name;
            }

            string _retunval = "";

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            // Gets list of claims.
            IEnumerable<Claim> claim = identity.Claims;
            var calimVal = claim
                               .Where(x => x.Type == name)
                               .FirstOrDefault();
            _retunval = Convert.ToString(calimVal.Value);
            return _retunval;
        }
    }
}