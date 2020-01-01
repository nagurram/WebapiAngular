using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataWebapi.DBContext;
using DataWebapi.Models;

namespace DataWebapi.api
{
    [Authorize]
    public class AdminapiController : BaseAPIController
    {

        public HttpResponseMessage Get()
        {
            return BAppMaster();
        }

        public HttpResponseMessage Post([FromBody]keyvalueModel value)
        {
            TicketDB.ApplicationMasters.Add(new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false });
            return ToJson(TicketDB.SaveChanges());
        }


        public HttpResponseMessage Put(int id, [FromBody]keyvalueModel value)
        {
            ApplicationMaster _applicationMaster = new ApplicationMaster() { ApplicationId = value.Id, ApplicationName = value.keyValue, IsDeleted = false };
            TicketDB.Entry(_applicationMaster).State = EntityState.Modified;
            return ToJson(TicketDB.SaveChanges());
        }


        public HttpResponseMessage Delete(int id)
        {
            TicketDB.ApplicationMasters.Remove(TicketDB.ApplicationMasters.FirstOrDefault(x => x.ApplicationId == id));
            return ToJson(TicketDB.SaveChanges());
        }



    }
}
