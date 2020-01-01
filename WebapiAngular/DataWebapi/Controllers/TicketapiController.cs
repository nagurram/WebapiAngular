using DataWebapi.DBContext;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace DataWebapi.api
{
    [RoutePrefix("api/Ticketapi")]
    [Authorize]
    public class TicketapiController : BaseAPIController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var _lstticket = from t in TicketDB.Tickets
                             join um in TicketDB.UserMasters on t.CreatedBy equals um.UserId
                             join pm in TicketDB.PriorityMasters on t.PriorityId equals pm.PriorityId
                             join am in TicketDB.ApplicationMasters on t.ApplicationId equals am.ApplicationId
                             join um2 in TicketDB.UserMasters on t.AssignedTo equals um2.UserId
                             join st in TicketDB.StatusMasters on t.StatusId equals st.StatusId
                             select new { t.TicketId, t.Title, t.Createddate, pm.PriorityDescription, createdby = um.LName + ", " + um.FName, am.ApplicationName, AssignedTo = um2.LName + ", " + um2.FName, status = st.StatusDescription };

            return ToJson(_lstticket);
        }

        [HttpPut, Route("Updateticket/{id}")]
        public HttpResponseMessage Put(int id, [FromBody]Ticket value)
        {
            Ticket _ticket = new Ticket() { TicketId = id, Title = value.Title, TDescription = value.TDescription, CreatedBy = value.CreatedBy, StatusId = value.StatusId, Createddate = value.Createddate, AssignedTo = value.AssignedTo, PriorityId = value.PriorityId, TypeId = value.TypeId, ApplicationId = value.ApplicationId, ModuleID = value.ModuleID, ResponseDeadline = value.ResponseDeadline, ResolutionDeadline = value.ResolutionDeadline, RootCauseId = value.RootCauseId, Coommnets = value.Coommnets, UpdatedBy = value.UpdatedBy, LastModifiedon = value.LastModifiedon };
            if (_ticket.TicketId == 0)
            {
                TicketDB.Tickets.Add(_ticket);
            }
            else
            {
                TicketDB.Entry(_ticket).State = System.Data.Entity.EntityState.Modified;
            }
            return ToJson(TicketDB.SaveChanges());
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            return ToJson(
                from t in
                TicketDB.Tickets
                where t.TicketId == id
                select new { t.TicketId, t.Title, t.TDescription, t.CreatedBy, t.StatusId, t.Createddate, t.AssignedTo, t.PriorityId, t.TypeId, t.ApplicationId, t.ModuleID, t.ResponseDeadline, t.ResolutionDeadline, t.RootCauseId, t.Coommnets, t.UpdatedBy, t.LastModifiedon }
                );
        }

        [HttpGet]
        [Route("AppMaster")]
        public HttpResponseMessage AppMaster()
        {
            return BAppMaster();
        }

        [HttpGet]
        [Route("ModuleMaster")]
        public HttpResponseMessage ModuleMaster()
        {
            return BModuleMaster();
        }

        [HttpGet]
        [Route("PriorityMaster")]
        public HttpResponseMessage PriorityMaster()
        {
            return BPriorityMaster();
        }

        [HttpGet]
        [Route("StatusMaster")]
        public HttpResponseMessage StatusMaster()
        {
            return BStatusMaster();
        }

        [HttpGet]
        [Route("UserMaster")]
        public HttpResponseMessage UserMaster()
        {
            return BUserMaster();
        }

        [HttpGet]
        [Route("TypeMaster")]
        public HttpResponseMessage TypeMaster()
        {
            return BTypeMaster();
        }
    }

}
