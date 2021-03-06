﻿using DataApi.DBContext;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using DataApi.common;
using System;
using System.Web;
using log4net;

namespace DataApi.api
{
    [RoutePrefix("api/userapi")]
    [Authorize]
    public class UserapiController : BaseAPIController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet, Route("GetMenuitems")]
        public HttpResponseMessage GetMenuitms()
        {

            Log.Info("This GetMenuitms method is called");
            int userid = Convert.ToInt32(GetClaimValue(Constants.UserId));
            var menus = (from p in TicketDB.vw_user_permissions
                         where p.userid == userid
                         select new { key= p.link, keyValue=p.displayname }).ToList();
            string userName = GetClaimValue(Constants.LastName) + ", " + GetClaimValue(Constants.FirstName);

            return ToJson(new { Userid = userid, UserName = userName, routeCollection = menus });
        }


       
        [HttpPost, AllowAnonymous]
        [Route("Logout")]
        public HttpResponseMessage Logout()
        {
            var authentication = HttpContext.Current.GetOwinContext().Authentication;
            authentication.SignOut();
            return ToJson(new { message = "Logout successful." });
        }
    }
}
