using LSDataApi.DBContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSDataApi.Services
{
    public interface IUserService
    {
        Resource Authenticate(string username, string password);

        Resource GetById(int id);

        IList<string> UserRoles(int userId);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> Log;

        public UserService(ILogger<UserService> logger)
        {
            Log = logger;
        }

        public Resource Authenticate(string username, string password)
        {
            Log.LogInformation("in Login, User id is:  " + username + " , Pwd " + "**********");
            // context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            Resource _logedinUser = null;
            //using (TicketTrackerContext TicketDB = new TicketTrackerContext())
            //{
            try
            {
                Log.LogInformation("step 1 of authentication");
                using (TicketTrackerContext TicketDB = new TicketTrackerContext())
                {
                    var user = TicketDB.Resource.SingleOrDefault(x => x.Email == username);

                    Log.LogInformation("step 2 of authentication");
                    if (user == null)
                    {
                        Log.LogInformation("invalid_grant", "The user name or password is incorrect.");
                        return _logedinUser;
                    }
                    else if (!user.Pwd.Equals(password, StringComparison.OrdinalIgnoreCase))
                    {
                        Log.LogInformation("invalid_grant", "The user name or password is incorrect.");
                        return _logedinUser;
                    }

                    user.Pwd = "";
                    _logedinUser = (Resource)user;
                }
                // authentication successful
                return _logedinUser;
                //}
            }
            catch (System.Exception e)
            {
                Log.LogError(e, "in Authenticate", null);
                return _logedinUser;
            }
        }

        public Resource GetById(int id)
        {
            Resource _logedinUser = null;
            Log.LogInformation("step 1 of authentication");
            using (TicketTrackerContext TicketDB = new TicketTrackerContext())
            {
                _logedinUser = TicketDB.Resource.Find(id);
            }
            return _logedinUser;
        }

        public IList<string> UserRoles(int userId)
        {
            IList<string> roles = null;
            using (TicketTrackerContext _repo = new TicketTrackerContext())
            {
                List<int> rolelist = (from r in _repo.UserRoles
                                      where r.UserId == userId
                                      select r.RoleId).ToList();

                roles = (from rm in _repo.RoleMaster
                         where rolelist.Contains(rm.RoleId)
                         select rm.RoleDescription).ToList();
            }
            return roles;
        }
    }
}