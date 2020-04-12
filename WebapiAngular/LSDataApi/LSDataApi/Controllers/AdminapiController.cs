using System;
using System.Collections.Generic;
using System.Linq;
using LSDataApi.DBContext;
using LSDataApi.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LSDataApi.DBContext;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace LSDataApi.api
{
    [Authorize(Roles = "Admin")]
    [Route("api/adminapi")]
    [EnableCors("_myAllowAllOrigins")]
    public class AdminapiController : BaseAPIController
    {
        private readonly ILogger<AdminapiController> Log;

        public AdminapiController(ILogger<AdminapiController> logger)
        {
            Log = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return await BAppMaster();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]keyvalueModel value)
        {
            TicketDB.ApplicationMaster.Add(new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false });
            return Ok(TicketDB.SaveChanges());
        }

        /// <summary>
        /// update Application details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut, Route("updateapplication/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]keyvalueModel value)
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
        public async Task<IActionResult> Delete(int id)
        {
            TicketDB.ApplicationMaster.Remove(TicketDB.ApplicationMaster.FirstOrDefault(x => x.ApplicationId == id));
            TicketDB.SaveChanges();
            return Ok(1);
        }

        /// <summary>
        /// Add user 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost, Route("adduser")]
        public async Task<IActionResult> adduser([FromBody]UserModel value)
        {
            var _lstResource = new List<Resource>();
            try
            {
                Log.LogInformation("Saving users");
                TicketDB.Resource.Add(new Resource() { Fname = value.FirstName, Lname = value.LastName, Email = value.EmailId, Pwd = "1234", Roles = "2", Isactive = true });
                TicketDB.SaveChanges();

                Log.LogInformation("Saving complete");
                return Ok(1);
            }
            catch (System.Exception e)
            {
                Log.LogError(null,e);
            }
            return Ok(0);
        }



    }
}