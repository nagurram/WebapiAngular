﻿using LsDataApi.Common;
using LSDataApi.DBContext;
using LSDataApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace LSDataApi.api
{
    /// <summary>
    /// Base api controller
    /// </summary>
    public class BaseApiController : ControllerBase
    {
        ///http://bitoftech.net/2015/03/11/asp-net-identity-2-1-roles-based-authorization-authentication-asp-net-web-api/
        /// http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
        /// http://www.dotnetmob.com/angular-5-tutorial/angular-5-login-and-logout-with-web-api-using-token-based-authentication/
        /// https://medium.com/aviabird/http-interceptor-angular2-way-e57dc2842462
        /// https://scotch.io/@kashyapmukkamala/using-http-interceptor-with-angular2
        /// https://www.youtube.com/watch?v=rbHSTJBhJ44
        /// https://medium.com/better-programming/creating-angular-webapp-for-multiple-views-and-screen-sizes-50fe8a83c433

        protected TicketTrackerContext TicketDB;

        /// <summary>
        /// application master list
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        protected IActionResult BAppMaster()
        {
            return Ok(TicketDB.ApplicationMaster.Select(p => new keyvalueModel { Id = p.ApplicationId, keyValue = p.ApplicationName, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        /// <summary>
        /// User master list
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        protected IActionResult BUserMaster()
        {
            return Ok(TicketDB.UserMaster.Select(p => new keyvalueModel { Id = p.UserId, keyValue = p.Fname + ", " + p.Lname, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        /// <summary>
        /// Status master list
        /// </summary>
        /// <returns></returns>
        protected IActionResult BStatusMaster()
        {
            return Ok(TicketDB.StatusMaster.Select(p => new keyvalueModel { Id = p.StatusId, keyValue = p.StatusDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        /// <summary>
        /// Priority master list
        /// </summary>
        /// <returns></returns>
        protected IActionResult BPriorityMaster()
        {
            return Ok(TicketDB.PriorityMaster.Select(p => new keyvalueModel { Id = p.PriorityId, keyValue = p.PriorityDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        /// <summary>
        /// Module master list
        /// </summary>
        /// <returns></returns>
        protected IActionResult BModuleMaster()
        {
            return Ok(TicketDB.ModuleMaster.Select(p => new keyvalueModel { Id = p.ModuleId, keyValue = p.ModuleName, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        /// <summary>
        /// Type master list
        /// </summary>
        /// <returns></returns>
        protected IActionResult BTypeMaster()
        {
            return Ok(TicketDB.TypeMaster.Select(p => new keyvalueModel { Id = p.TypeId, keyValue = p.TypeDescription, IsDeleted = p.IsDeleted }).AsEnumerable());
        }

        /// <summary>
        /// Rootcause master list
        /// </summary>
        /// <returns></returns>
        protected IActionResult BRootCauseMaster()
        {
            return Ok(TicketDB.RootCauseMaster.Select(p => new keyvalueModel { Id = p.RootCauseId, keyValue = p.Description, IsDeleted = p.Isdelete }).AsEnumerable());
        }

        /// <summary>
        /// User Claims list
        /// </summary>
        /// <returns></returns>
        protected string GetClaimValue(string name)
        {
            if (name == Constants.UserId)
            {
                name = ClaimTypes.Name;
            }

            string _retunval = "";

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            // Gets list of claims.
            var calimVal = identity.Claims.Where(x => x.Type == name).FirstOrDefault();
            _retunval = Convert.ToString(calimVal.Value);
            return _retunval;
        }
    }
}