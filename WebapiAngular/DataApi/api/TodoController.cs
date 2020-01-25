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
using log4net;
using System.Web.Http.Cors;

namespace DataApi.api
{
    [RoutePrefix("api/Todoapi")]
   // [EnableCors(origins: "http://vmtest.australiaeast.cloudapp.azure.com/angtodo", headers: "*", methods: "*")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    public class TodoController : BaseAPIController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Returns All Todo List items
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("todolist")]
        public HttpResponseMessage Get()
        {
            var _lsttodoe = new List<TodoList>();
            try
            {
              var   _lsttodo = (from t in TicketDB.TodoLists

                               select new { t.TodoId, t.Userid, t.Titile, t.Description, t.actionDate, t.IsActive }).ToList();
                return ToJson(_lsttodo);
            }
            catch(System.Exception e)
            {
                Log.Error(e);
            }
            return ToJson(_lsttodoe);
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
