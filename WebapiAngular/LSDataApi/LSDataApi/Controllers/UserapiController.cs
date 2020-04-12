using System;
using System.Collections.Generic;
using System.Linq;
using LSDataApi.DBContext;
using LSDataApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LSDataApi.DBContext;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using LSDataApi.api;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using LsDataApi.Common;
using System.Text;
using System.Net.Http;
using LSDataApi.Services;
using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace LSDataApi.api
{
    [Route("api/userapi")]
    [Authorize]
    [ApiController]
    [EnableCors("_myAllowAllOrigins")]
    public class UserapiController : BaseAPIController
    {
        private readonly ILogger<UserapiController> Log;
        private IUserService _userService;
        private readonly AppSettings _appSettings;
        public UserapiController(ILogger<UserapiController> logger, IUserService userService, IOptions<AppSettings> appSettings)
        {
            Log = logger;
            _userService = userService;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateModel model)
        {

            Log.LogInformation("User name is " + model.Username);
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.ResourceId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
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


        [HttpGet, Route("GetMenuitems")]
        public async Task<IActionResult> GetMenuitms()
        {

            Log.LogInformation("This GetMenuitms method is called");
            int userid = Convert.ToInt32(GetClaimValue(Constants.UserId));
            var menus = (from p in TicketDB.VwUserPermissions
                         where p.Userid == userid
                         select new { key = p.Link, keyValue = p.Displayname }).ToList();
            string userName = GetClaimValue(Constants.LastName) + ", " + GetClaimValue(Constants.FirstName);

            return Ok(new { Userid = userid, UserName = userName, routeCollection = menus });
        }



        [HttpPost, AllowAnonymous]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // var authentication = HttpContext.Current.GetOwinContext().Authentication;
            //authentication.SignOut();
            return Ok(new { message = "Logout successful." });
        }
    }
}
