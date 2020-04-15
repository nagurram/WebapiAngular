using LSDataApi.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSDataApi.api
{
    [Route("api/Todoapi")]
    // [EnableCors(origins: "http://vmtest.australiaeast.cloudapp.azure.com/angtodo", headers: "*", methods: "*")]
    [EnableCors("_myAllowAllOrigins")]
    [Authorize]
    public class TodoController : BaseAPIController
    {
        private readonly ILogger<TodoController> Log;

        public TodoController(ILogger<TodoController> logger)
        {
            Log = logger;
        }

        /// <summary>
        /// Returns All Todo List items
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("todolist")]
        public async Task<IActionResult> Get()
        {
            var _lsttodoe = new List<TodoList>();
            try
            {
                var _lsttodo = (from t in TicketDB.TodoList

                                select new { t.TodoId, t.Userid, t.Titile, t.Description, t.ActionDate, t.IsActive }).ToList();
                return Ok(_lsttodo);
            }
            catch (System.Exception e)
            {
                Log.LogError(null, e);
            }
            return Ok(_lsttodoe);
        }

        [HttpPut, Route("Updatetodo/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]TodoList value)
        {
            TodoList _todoitem = new TodoList() { TodoId = value.TodoId, Userid = value.Userid, Titile = value.Titile, Description = value.Description, ActionDate = value.ActionDate, IsActive = value.IsActive };
            if (_todoitem.TodoId == 0)
            {
                TicketDB.TodoList.Add(_todoitem);
            }
            else
            {
                TicketDB.Entry(_todoitem).State = EntityState.Modified;
            }
            return Ok(TicketDB.SaveChanges());
        }
    }
}