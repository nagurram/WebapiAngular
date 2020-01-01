using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ang4api.DBContext;
using ang4api.Models;

namespace ang4api.api
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


        [HttpPut, Route("updateapplication/{id}")]
        public HttpResponseMessage Put(int id, [FromBody]keyvalueModel value)
        {
            ApplicationMaster _applicationMaster = new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false };
            TicketDB.Entry(_applicationMaster).State = EntityState.Modified;
            return ToJson(TicketDB.SaveChanges());
        }


        [HttpDelete, Route("deleteapplication/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            TicketDB.ApplicationMasters.Remove(TicketDB.ApplicationMasters.FirstOrDefault(x => x.ApplicationId == id));
            return ToJson(TicketDB.SaveChanges());
        }

        [HttpPost, Route("adduser")]
        public HttpResponseMessage adduser([FromBody]UserModel value)
        {
            TicketDB.Resources.Add(new Resource() { FName = value.FirstName, Lname = value.LastName, Email = value.EmailId,Pwd="1234",Roles="2",Isactive=true });
            return ToJson(TicketDB.SaveChanges());
        }



    }
}