using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataApi.DBContext;
using DataApi.Models;

namespace DataApi.api
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/adminapi")]
    public class AdminapiController : BaseAPIController
    {

        public HttpResponseMessage Get()
        {
            return BAppMaster();
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]keyvalueModel value)
        {
            TicketDB.ApplicationMasters.Add(new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false });
            return ToJson(TicketDB.SaveChanges());
        }

        /// <summary>
        /// update Application details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut, Route("updateapplication/{id}")]
        public HttpResponseMessage Put(int id, [FromBody]keyvalueModel value)
        {
            ApplicationMaster _applicationMaster = new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false };
            TicketDB.Entry(_applicationMaster).State = EntityState.Modified;
            return ToJson(TicketDB.SaveChanges());
        }

        /// <summary>
        /// Delete Application
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("deleteapplication/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            TicketDB.ApplicationMasters.Remove(TicketDB.ApplicationMasters.FirstOrDefault(x => x.ApplicationId == id));
            return ToJson(TicketDB.SaveChanges());
        }

        /// <summary>
        /// Add user 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost, Route("adduser")]
        public HttpResponseMessage adduser([FromBody]UserModel value)
        {
            TicketDB.Resources.Add(new Resource() { FName = value.FirstName, Lname = value.LastName, Email = value.EmailId,Pwd="1234",Roles="2",Isactive=true });
            return ToJson(TicketDB.SaveChanges());
        }



    }
}