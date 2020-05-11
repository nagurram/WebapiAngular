using LsDataApi.Common;
using LSDataApi.DBContext;
using LSDataApi.Models;
using LSDataApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApi.Helpers;

namespace LSDataApi.api
{
    [Route("api/userapi")]
    [Authorize]
    [EnableCors("_myAllowAllOrigins")]
    public class UserapiController : BaseAPIController
    {
        private readonly ILogger<UserapiController> Log;
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public UserapiController(ILogger<UserapiController> logger, IUserService userService, IOptions<AppSettings> appSettings, TicketTrackerContext context)
        {
            TicketDB = context;
            Log = logger;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            Log.LogInformation("User name is " + model.Username);
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            IList<string> _roles = _userService.UserRoles(user.ResourceId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.ResourceId.ToString()),
                     new Claim(Constants.FirstName, user.Fname),
                     new Claim(Constants.LastName, user.Lname)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            tokenDescriptor.Subject.AddClaims(_roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                UserId = user.ResourceId,
                Emailid = user.Email,
                FirstName = user.Fname,
                LastName = user.Lname,
                Token = tokenString
            });
        }

        /// <summary>
        /// Get menus
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetMenuitems")]
        public IActionResult GetMenuitms()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            // Gets list of claims.
            IEnumerable<Claim> claim = identity.Claims;

            var lastName = GetClaimValue(Constants.LastName);
            var firstName = GetClaimValue(Constants.FirstName);
            var id = GetClaimValue(ClaimTypes.Name);

            Log.LogInformation("This GetMenuitms method is called");
            int userid = Convert.ToInt32(id);
            var menus = (from p in TicketDB.VwUserPermissions
                         where p.Userid == userid
                         orderby p.Sortorder
                         select new { key = p.Link, keyValue = p.Displayname }).ToList();
            string userName = Convert.ToString(lastName) + ", " + Convert.ToString(firstName);

            return Ok(new { Userid = userid, UserName = userName, routeCollection = menus });
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // var authentication = HttpContext.Current.GetOwinContext().Authentication;
            //authentication.SignOut();
            return Ok(new { message = "Logout successful." });
        }
    }
}