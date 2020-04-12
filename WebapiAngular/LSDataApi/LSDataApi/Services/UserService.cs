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
            Log.LogInformation("step 1 of authentication");
            using (TicketTrackerContext TicketDB = new TicketTrackerContext())
            {
                var user = TicketDB.Resource.Where(c => c.Email.Equals(username, StringComparison.OrdinalIgnoreCase) && c.Pwd.Equals(password))
                    .FirstOrDefault();
                //var user = (from p in TicketDB.Resources
                //            where p.Email.Equals(context.UserName, StringComparison.OrdinalIgnoreCase) && p.Pwd.Equals(context.Password)
                //            select p).FirstOrDefault();
                Log.LogInformation("step 2 of authentication");
                if (user == null)
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
    }
}