using System;
using System.Linq;
using System.Threading.Tasks;
using LSDataApi.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace LSDataApi.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private TicketTrackerContext TicketDB;
        private IConfiguration configuration;

        public TokenManager(TicketTrackerContext context,
                IHttpContextAccessor httpContextAccessor,
                IConfiguration iConfig
            )
        {
            TicketDB = context;
            _httpContextAccessor = httpContextAccessor;
            configuration = iConfig;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task<bool> Skipvalidation(string path)
        {
            var lstpath = configuration.GetSection("MiddlewareSkips:PathList").Value.ToString().ToLower().Split(',').ToList();
            if (lstpath.Contains(path.ToLower()))
            {
                return true;
            }
            return false;
        }

        public async Task DeactivateCurrentAsync()

            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
        {
            var result = TicketDB.UserToken.SingleOrDefault(b => b.AccessToken == token && b.TokenValidUntil > DateTime.UtcNow);
            bool isactive = result != null ? true : false;
            return isactive;
        }

        public async Task SaveToken(int userid, string accesstoken, string secret)
        {
            try
            {
                TicketDB.UserToken.Add(new UserToken() { AccessToken = accesstoken, PrivateKey = secret, ResourceId = userid, TokenValidFrom = DateTime.UtcNow, TokenValidUntil = DateTime.UtcNow.AddDays(1) });
                TicketDB.SaveChanges();
            }
            catch (System.Exception e)
            {
            }
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

        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}