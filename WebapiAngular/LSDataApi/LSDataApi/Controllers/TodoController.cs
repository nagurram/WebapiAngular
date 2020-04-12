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
              var   _lsttodo = (from t in TicketDB.TodoList

                               select new { t.TodoId, t.Userid, t.Titile, t.Description, t.ActionDate, t.IsActive }).ToList();
                return Ok(_lsttodo);
            }
            catch(System.Exception e)
            {
                Log.LogError(null,e);
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
                TicketDB.Entry(_todoitem).State =  EntityState.Modified;
            }
            return Ok(TicketDB.SaveChanges());
        }


    }
}
