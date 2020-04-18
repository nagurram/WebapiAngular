using LSDataApi;
using LSDataApi.api;
using LSDataApi.DBContext;
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
using System.Threading.Tasks;

namespace DataApi.api
{
    [Microsoft.AspNetCore.Mvc.Route("api/Ticketapi")]
    [Authorize(Roles = "Admin,BasicUser")]
    [EnableCors("_myAllowAllOrigins")]
    public class TicketapiController : BaseAPIController
    {
        private readonly ILogger<TicketapiController> Log;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string diskFolderPath = "";

        public TicketapiController(ILogger<TicketapiController> logger, IWebHostEnvironment hostingEnvironment)
        {
            Log = logger;
            _hostingEnvironment = hostingEnvironment;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            diskFolderPath = Path.Combine(contentRootPath, "App_Data");
        }

        #region "Basic data"

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var _lstticket = from t in TicketDB.Tickets
                             join um in TicketDB.UserMaster on t.CreatedBy equals um.UserId
                             join pm in TicketDB.PriorityMaster on t.PriorityId equals pm.PriorityId
                             join am in TicketDB.ApplicationMaster on t.ApplicationId equals am.ApplicationId
                             join um2 in TicketDB.UserMaster on t.AssignedTo equals um2.UserId
                             join st in TicketDB.StatusMaster on t.StatusId equals st.StatusId
                             join tp in TicketDB.TypeMaster on t.TypeId equals tp.TypeId
                             select new { t.TicketId, t.Title, t.Createddate, pm.PriorityDescription, createdby = um.Lname + ", " + um.Fname, am.ApplicationName, AssignedTo = um2.Lname + ", " + um2.Fname, status = st.StatusDescription, tkttype = tp.TypeDescription };

            return Ok(_lstticket);
        }

        [HttpPut, Route("Updateticket/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Tickets value)
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(
                from t in
                TicketDB.Tickets
                where t.TicketId == id
                select new { t.TicketId, t.Title, t.Tdescription, t.CreatedBy, t.StatusId, t.Createddate, t.AssignedTo, t.PriorityId, t.TypeId, t.ApplicationId, t.ModuleId, t.ResponseDeadline, t.ResolutionDeadline, t.RootCauseId, t.Comments, t.UpdatedBy, t.LastModifiedon }
                );
        }

        [HttpGet]
        [Route("AppMaster")]
        public async Task<IActionResult> AppMaster()
        {
            return await BAppMaster();
        }

        [HttpGet]
        [Route("RootcauseMaster")]
        public async Task<IActionResult> RootcauseMaster()
        {
            return await BRootCauseMaster();
        }

        [HttpGet]
        [Route("ModuleMaster")]
        public async Task<IActionResult> ModuleMaster()
        {
            return await BModuleMaster();
        }

        [HttpGet]
        [Route("PriorityMaster")]
        public async Task<IActionResult> PriorityMaster()
        {
            return await BPriorityMaster();
        }

        [HttpGet]
        [Route("StatusMaster")]
        public async Task<IActionResult> StatusMaster()
        {
            return await BStatusMaster();
        }

        [HttpGet]
        [Route("UserMaster")]
        public async Task<IActionResult> UserMaster()
        {
            return await BUserMaster();
        }

        [HttpGet]
        [Route("TypeMaster")]
        public async Task<IActionResult> TypeMaster()
        {
            return await BTypeMaster();
        }

        #endregion "Basic data"

        #region "file upload"

        [HttpGet, Route("Getattachments/{id}")]
        public async Task<IActionResult> GetTicketAttachemnets(int id)
        {
            var filelist = (TicketDB.FileUpload.Where(t => t.TicketId == id).Select(p => new { p.Fileid, p.FileName, p.Filetype, p.UploadDate })).ToList();
            return Ok(filelist);
        }

        [HttpPost, Route("Uploadattachments/{id}"), DisableRequestSizeLimit]
        public IActionResult PostFormDataAsync(int id)
        {
            var path = Path.GetTempPath();
            var file = Request.Form.Files[0];

            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);
            try
            {
                if (file.Length > 0)
                {
                    string fileName = "";
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        fileName = Guid.NewGuid().ToString();
                    }
                    fileName = file.FileName;
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

        [HttpGet, Route("GetfileAttachemnet/{id}")]
        public IActionResult GetfileAttachemnet(int id)
        {
            Log.LogInformation("in GetfileAttachemnet  method and file id is :" + id);
            var filelist = (TicketDB.FileUpload.Where(t => t.Fileid == id).Select(p => new { p.Fileid, p.Filedata, p.FileName, p.Filetype, p.UploadDate })).FirstOrDefault();

            var memorycontent = new MemoryStream(filelist.Filedata);
            //  string cotenttype = GetfileContenttype(filelist.Filetype);

            string fileres = Encoding.UTF8.GetString(filelist.Filedata, 0, filelist.Filedata.Length);
            StreamContent _rescontent = new StreamContent(memorycontent); ;// new StringContent(JsonConvert.SerializeObject(fileres), Encoding.UTF8, cotenttype);
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