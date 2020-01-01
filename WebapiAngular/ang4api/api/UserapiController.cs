using ang4api.DBContext;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ang4api.common;
using System;
using System.Web;

namespace ang4api.api
{
    [RoutePrefix("api/userapi")]
    [Authorize]
    public class UserapiController : BaseAPIController
    {

        [HttpGet, Route("GetMenuitems")]
        public HttpResponseMessage GetMenuitms()
        {
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
