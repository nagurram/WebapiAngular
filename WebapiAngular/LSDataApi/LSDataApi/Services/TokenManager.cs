using LSDataApi.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LSDataApi.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration configuration;
        private TicketTrackerContext TicketDB;

        public TokenManager(TicketTrackerContext context,
                IHttpContextAccessor httpContextAccessor,
                IConfiguration iConfig
            )
        {
            TicketDB = context;
            _httpContextAccessor = httpContextAccessor;
            configuration = iConfig;
        }

        public async Task DeactivateAsync(string token)
        {
            var result = TicketDB.UserToken.SingleOrDefault(b => b.AccessToken == token);
            if (result != null)
            {
                result.TokenValidUntil = DateTime.UtcNow;
                TicketDB.SaveChanges();
            }
        }

        public async Task DeactivateCurrentAsync()

            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
        {
            var result = TicketDB.UserToken.SingleOrDefault(b => b.AccessToken == token && b.TokenValidUntil > DateTime.UtcNow);
            bool isactive = result != null ? true : false;
            return isactive;
        }

        public async Task<bool> IsCurrentActiveToken()
                                    => await IsActiveAsync(GetCurrentAsync());

        public async Task SaveToken(int userid, string accesstoken, string secret)
        {
            TicketDB.UserToken.Add(new UserToken() { AccessToken = accesstoken, PrivateKey = secret, ResourceId = userid, TokenValidFrom = DateTime.UtcNow, TokenValidUntil = DateTime.UtcNow.AddDays(1) });
            TicketDB.SaveChanges();
        }

        public async Task<bool> Skipvalidation(string path)
        {
            var lstpath = configuration.GetSection("MiddlewareSkips:PathList").Value.ToString().ToLower().Split(',').ToList();
            if (lstpath.Contains(path.ToLower()))
            {
                return true;
            }
            return false;
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";

        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }
    }
}