﻿using ang4api.DBContext;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ang4api.common;

namespace ang4api.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                Resource _logedinUser = null;
                using (TicketTrackerEntities _repo = new TicketTrackerEntities())
                {
                    var user = (from p in _repo.Resources
                                where p.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) && p.Pwd.Equals(context.Password)
                                select p).FirstOrDefault();

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
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        private IList<string> UserRoles(int userId)
        {
            IList<string> roles = null;
            using (TicketTrackerEntities _repo = new TicketTrackerEntities())
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