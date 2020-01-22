using DataApi.DBContext;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace DataApi.api
{
    [RoutePrefix("api/Todoapi")]
    public class TodoController : BaseAPIController
    {

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var _lsttodo = from t in TicketDB.TodoLists

                           select new { t.TodoId, t.Userid, t.Titile, t.Description, t.actionDate, t.IsActive };

            return ToJson(_lsttodo);
        }


        [HttpPut, Route("Updatetodo/{id}")]
        public HttpResponseMessage Put(int id, [FromBody]TodoList value)
        {
            TodoList _todoitem = new TodoList() { TodoId = value.TodoId, Userid = value.Userid, Titile = value.Titile, Description = value.Description, actionDate = value.actionDate, IsActive = value.IsActive };
            if (_todoitem.TodoId == 0)
            {
                TicketDB.TodoLists.Add(_todoitem);
            }
            else
            {
                TicketDB.Entry(_todoitem).State = System.Data.Entity.EntityState.Modified;
            }
            return ToJson(TicketDB.SaveChanges());
        }


    }
}
