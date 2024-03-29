﻿/*
using LSDataApi.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LSDataApi.common;

namespace LSDataApi.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                Log.Info("in Login, User id is:  " + context.UserName + " , Pwd " + context.Password);
               // context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                Resource _logedinUser = null;
                using (TicketTrackerEntities2 _repo = new TicketTrackerEntities2())
                {
                    Log.Info("step 1 of authentication");

                    var user = _repo.Resources.Where(c => c.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) && c.Pwd.Equals(context.Password))
                        .FirstOrDefault();
                    //var user = (from p in _repo.Resources
                    //            where p.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) && p.Pwd.Equals(context.Password)
                    //            select p).FirstOrDefault();
                    Log.Info("step 2 of authentication");
                    if (user == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                    _logedinUser = (Resource)user;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(Constants.Email, context.UserName));
                identity.AddClaim(new Claim(Constants.UserId, _logedinUser.ResourceId.ToString()));
                identity.AddClaim(new Claim(Constants.FirstName, _logedinUser.FName));
                identity.AddClaim(new Claim(Constants.LastName, _logedinUser.Lname));
                identity.AddClaim(new Claim(Constants.LoggedOn, DateTime.Now.ToString()));
                var _userroles = UserRoles(_logedinUser.ResourceId);
                foreach (string rl in _userroles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, rl));
                }
                context.Validated(identity);
                Log.Info("end of authentication");
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private IList<string> UserRoles(int userId)
        {
            IList<string> roles = null;
            using (TicketTrackerEntities2 _repo = new TicketTrackerEntities2())
            {
                List<int> rolelist = (from r in _repo.UserRoles
                                      where r.UserId == userId
                                      select r.RoleId).ToList();

                roles = (from rm in _repo.RoleMasters
                         where rolelist.Contains(rm.RoleId)
                         select rm.RoleDescription).ToList();
            }
            return roles;
        }
    }
}
*/