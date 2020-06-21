using LSDataApi.DBContext;
using LSDataApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSDataApi.api
{
    /// <summary>
    /// Adminapi Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route("api/adminapi")]
    [EnableCors("_myAllowAllOrigins")]
    public class AdminapiController : BaseApiController
    {
        private readonly ILogger<AdminapiController> Log;

        /// <summary>
        /// Constructor Dependency injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public AdminapiController(ILogger<AdminapiController> logger, TicketTrackerContext context)
        {
            TicketDB = context;
            Log = logger;
        }

        /// <summary>
        /// Gets list of applications
        /// </summary>
        /// <returns></returns>
        [HttpGet, ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return BAppMaster();
        }

        /// <summary>
        /// Gets list of application  by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(TicketDB.ApplicationMaster.Find(id));
        }

        /// <summary>
        /// Create new Application
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] keyvalueModel value)
        {
            var app = new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false };
            TicketDB.ApplicationMaster.Add(app);
            TicketDB.SaveChanges();
            TicketDB.Dispose();
            return CreatedAtAction(nameof(GetById), new { id = app.ApplicationId });
        }

        /// <summary>
        /// update Application details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut, Route("updateapplication/{id}")]
        public IActionResult Put(int id, [FromBody] keyvalueModel value)
        {
            ApplicationMaster _applicationMaster = new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false };
            TicketDB.Entry(_applicationMaster).State = EntityState.Modified;
            return Ok(TicketDB.SaveChanges());
        }

        /// <summary>
        /// Delete Application
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("deleteapplication/{id}")]
        public IActionResult Delete(int id)
        {
            TicketDB.ApplicationMaster.Remove(TicketDB.ApplicationMaster.FirstOrDefault(x => x.ApplicationId == id));
            TicketDB.SaveChanges();
            return Ok(1);
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost, Route("adduser")]
        public IActionResult adduser([FromBody] UserModel value)
        {
            try
            {
                if (value.FirstName == null || value.LastName == null || value.EmailId == null || value.Roleid == null)
                {
                    throw new InvalidOperationException("invalid user details");
                }
                Log.LogInformation("Saving users");
                TicketDB.Resource.Add(new Resource() { Fname = value.FirstName, Lname = value.LastName, Email = value.EmailId, Pwd = "1234", Roles = "2", Isactive = true });
                TicketDB.SaveChanges();
                Log.LogInformation("Saving complete");
                return Ok(1);
            }
            catch (System.Exception e)
            {
                Log.LogError(null, e);
                return ValidationProblem("something wrong with user details " + e.Message);
            }
        }
    }
}