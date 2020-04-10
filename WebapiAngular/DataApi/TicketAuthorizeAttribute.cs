using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataApi
{
    /// <summary>
    /// Ticket Authorization per user
    /// </summary>
    public class TicketAuthorizeAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
       public void OnAuthorization(AuthorizationContext filterContext)
        {

        }
    }
}