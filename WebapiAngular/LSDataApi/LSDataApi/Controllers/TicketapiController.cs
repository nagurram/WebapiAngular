using LSDataApi;
using LSDataApi.api;
using LSDataApi.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace DataApi.api
{
    [Microsoft.AspNetCore.Mvc.Route("api/Ticketapi")]
    [EnableCors("_myAllowAllOrigins")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,BasicUser")]
    public class TicketapiController : BaseAPIController
    {
        private readonly ILogger<TicketapiController> Log;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string diskFolderPath = "";

        public TicketapiController(ILogger<TicketapiController> logger, IWebHostEnvironment hostingEnvironment, TicketTrackerContext context)
        {
            Log = logger;
            TicketDB = context;
            _hostingEnvironment = hostingEnvironment;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            diskFolderPath = Path.Combine(contentRootPath, "App_Data");
        }

        #region "Basic data"

        /// <summary>
        /// This method is for getting all tickets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var _lstticket = from t in TicketDB.Tickets
                             join um in TicketDB.UserMaster on t.CreatedBy equals um.UserId into um
                             from subum in um.DefaultIfEmpty()
                             join pm in TicketDB.PriorityMaster on t.PriorityId equals pm.PriorityId
                             join am in TicketDB.ApplicationMaster on t.ApplicationId equals am.ApplicationId
                             join um2 in TicketDB.UserMaster on t.AssignedTo equals um2.UserId
                             join st in TicketDB.StatusMaster on t.StatusId equals st.StatusId
                             join tp in TicketDB.TypeMaster on t.TypeId equals tp.TypeId
                             select new { t.TicketId, t.Title, t.Createddate, pm.PriorityDescription, createdby = subum.Lname + ", " + subum.Fname, am.ApplicationName, AssignedTo = um2.Lname + ", " + um2.Fname, status = st.StatusDescription, tkttype = tp.TypeDescription };

            return Ok(_lstticket);
        }

        /// <summary>
        /// Update ticket details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut, Route("Updateticket/{id}")]
        public IActionResult Put(int id, [FromBody]Tickets value)
        {
            Tickets _ticket = new Tickets() { TicketId = id, Title = value.Title, Tdescription = value.Tdescription, CreatedBy = value.CreatedBy, StatusId = value.StatusId, Createddate = value.Createddate, AssignedTo = value.AssignedTo, PriorityId = value.PriorityId, TypeId = value.TypeId, ApplicationId = value.ApplicationId, ModuleId = value.ModuleId, ResponseDeadline = value.ResponseDeadline, ResolutionDeadline = value.ResolutionDeadline, RootCauseId = value.RootCauseId, Comments = value.Comments, UpdatedBy = Convert.ToInt32(GetClaimValue(ClaimTypes.Name)), LastModifiedon = value.LastModifiedon };
            if (_ticket.TicketId == 0)
            {
                TicketDB.Tickets.Add(_ticket);
            }
            else
            {
                TicketDB.Entry(_ticket).State = EntityState.Modified;
            }
            return Ok(TicketDB.SaveChanges());
        }

        /// <summary>
        /// Get ticket by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(
                from t in
                TicketDB.Tickets
                where t.TicketId == id
                select new { t.TicketId, t.Title, t.Tdescription, t.CreatedBy, t.StatusId, t.Createddate, t.AssignedTo, t.PriorityId, t.TypeId, t.ApplicationId, t.ModuleId, t.ResponseDeadline, t.ResolutionDeadline, t.RootCauseId, t.Comments, t.UpdatedBy, t.LastModifiedon }
                );
        }

        /// <summary>
        /// Get all Applications list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AppMaster")]
        public IActionResult AppMaster()
        {
            return BAppMaster();
        }

        /// <summary>
        /// Gets all Rootcause list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RootcauseMaster")]
        public IActionResult RootcauseMaster()
        {
            return BRootCauseMaster();
        }

        /// <summary>
        /// Gets all Module list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ModuleMaster")]
        public IActionResult ModuleMaster()
        {
            return BModuleMaster();
        }

        /// <summary>
        /// Gets all Priority list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PriorityMaster")]
        public IActionResult PriorityMaster()
        {
            return BPriorityMaster();
        }

        /// <summary>
        /// Gets all Status list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("StatusMaster")]
        public IActionResult StatusMaster()
        {
            return BStatusMaster();
        }

        /// <summary>
        /// Gets all User list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UserMaster")]
        public IActionResult UserMaster()
        {
            return BUserMaster();
        }

        /// <summary>
        /// Gets all Type list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TypeMaster")]
        public IActionResult TypeMaster()
        {
            return BTypeMaster();
        }

        #endregion "Basic data"

        #region "file upload"

        /// <summary>
        /// Gets attachment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("Getattachments/{id}")]
        public IActionResult GetTicketAttachemnets(int id)
        {
            var filelist = (TicketDB.FileUpload.Where(t => t.TicketId == id).Select(p => new { p.Fileid, p.FileName, p.Filetype, p.UploadDate })).ToList();
            return Ok(filelist);
        }

        /// <summary>
        /// Upload attachment by ticket id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("Uploadattachments/{id}"), DisableRequestSizeLimit]
        public IActionResult PostFormData(int id)
        {
            var path = Path.GetTempPath();
            var file = Request.Form.Files[0];

            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);
            try
            {
                if (file.Length > 0)
                {
                    string fileName = string.IsNullOrEmpty(file.FileName) ? Guid.NewGuid().ToString() : file.FileName;

                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    var newFileName = Path.Combine(diskFolderPath, fileName);
                    var fileInfo = new FileInfo(newFileName);
                    if (fileInfo.Exists)
                    {
                        fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                        fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                        newFileName = Path.Combine(diskFolderPath, fileName);
                    }

                    if (!Directory.Exists(fileInfo.Directory.FullName))
                    {
                        Directory.CreateDirectory(fileInfo.Directory.FullName);
                    }
                    using (var stream = new FileStream(newFileName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    SaveToDB(newFileName, id);
                }
                return Ok(new { Message = "1" });
            }
            catch (System.Exception e)
            {
                Log.LogError(null, e);
                return Ok(new { Message = e.StackTrace });
            }
        }

        /// <summary>
        /// Download attachment by attachment  by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetfileAttachemnet/{id}")]
        public IActionResult GetfileAttachemnet(int id)
        {
            Log.LogInformation("in GetfileAttachemnet  method and file id is :" + id);
            var filelist = (TicketDB.FileUpload.Where(t => t.Fileid == id).Select(p => new { p.Fileid, p.Filedata, p.FileName, p.Filetype, p.UploadDate })).FirstOrDefault();

            var memorycontent = new MemoryStream(filelist.Filedata);

            string fileres = Encoding.UTF8.GetString(filelist.Filedata, 0, filelist.Filedata.Length);
            StreamContent _rescontent = new StreamContent(memorycontent);
            Log.LogInformation("in GetfileAttachemnet  method in last line");

            return File(memorycontent, new DownloadResult().GetfileContenttype(filelist.Filetype), filelist.FileName);
        }

        private int SaveToDB(string filename, int ticketid)
        {
            try
            {
                FileInfo fileStream = new FileInfo(filename);

                var fileupload = new FileUpload();

                byte[] filecontent = new byte[fileStream.Length];
                FileStream imagestream = fileStream.OpenRead();
                imagestream.Read(filecontent, 0, filecontent.Length);
                imagestream.Close();
                fileupload.Filedata = filecontent;
                fileupload.FileName = fileStream.Name;
                fileupload.UploadDate = DateTime.Now;
                fileupload.Filetype = fileStream.Name.Split('.')[1].ToString();
                fileupload.TicketId = ticketid;
                TicketDB.FileUpload.Add(fileupload);
                TicketDB.SaveChanges();

                if ((System.IO.File.Exists(filename)))
                {
                    System.IO.File.Delete(filename);
                }

                return 1;
            }
            catch (System.Exception e)
            {
                Log.LogDebug(e.Message);
                return 0;
            }
        }

        #endregion "file upload"
    }
}