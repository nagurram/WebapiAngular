using DataWebapi.DBContext;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DataWebapi.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            Resource _logedinUser = null;
            using (TicketTrackerEntities _repo = new TicketTrackerEntities())
            {
                var user = (from p in _repo.Resources
                           where p.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) && p.Pwd.Equals(context.Password) 
                           select  p).FirstOrDefault();

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                _logedinUser = (Resource)user;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("email", context.UserName));
            identity.AddClaim(new Claim("FirstName", _logedinUser.FName));
            identity.AddClaim(new Claim("LastName", _logedinUser.Lname));
            identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
            var _userroles = await UserRoles(_logedinUser.ResourceId);
            foreach (string rl in _userroles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, rl));
            }
            context.Validated(identity);

        }

        private async Task<IList<string>> UserRoles(int userId)
        {
            IList<string> roles = null;
            using (TicketTrackerEntities _repo = new TicketTrackerEntities())
            {

                var rolelist = (from r in _repo.Resources
                                where r.ResourceId == userId
                                select r.Roles).FirstOrDefault();

                List<int> roleids = rolelist.Split(',').Select(int.Parse).ToList();


                roles = (from rm in _repo.RoleMasters
                         where roleids.Contains(rm.RoleId)
                         select rm.RoleDescription).ToList();

            }
            return roles;
        }
    }
}