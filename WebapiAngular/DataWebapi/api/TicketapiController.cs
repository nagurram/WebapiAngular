using DataWebapi.DBContext;
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

namespace DataWebapi.api
{

    [RoutePrefix("api/Ticketapi")]
    [Authorize(Roles = "Admin,BasicUser")]
    public class TicketapiController : BaseAPIController
    {


        string diskFolderPath = HttpContext.Current.Server.MapPath("~/App_Data");


        #region "Basic data"
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var _lstticket = from t in TicketDB.Tickets
                             join um in TicketDB.UserMasters on t.CreatedBy equals um.UserId
                             join pm in TicketDB.PriorityMasters on t.PriorityId equals pm.PriorityId
                             join am in TicketDB.ApplicationMasters on t.ApplicationId equals am.ApplicationId
                             join um2 in TicketDB.UserMasters on t.AssignedTo equals um2.UserId
                             join st in TicketDB.StatusMasters on t.StatusId equals st.StatusId
                             join tp in TicketDB.TypeMasters on t.TypeId equals tp.TypeId
                             select new { t.TicketId, t.Title, t.Createddate, pm.PriorityDescription, createdby = um.LName + ", " + um.FName, am.ApplicationName, AssignedTo = um2.LName + ", " + um2.FName, status = st.StatusDescription, tkttype = tp.TypeDescription };

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
        [Route("RootcauseMaster")]
        public HttpResponseMessage RootcauseMaster()
        {
            return BRootCauseMaster();
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
        #endregion

        #region "file upload"

        [HttpGet, Route("Getattachments/{id}")]
        public HttpResponseMessage GetTicketAttachemnets(int id)
        {
            var filelist = (TicketDB.FileUploads.Where(t => t.TicketId == id).Select(p => new { p.Fileid, p.FileName, p.Filetype, p.UploadDate })).ToList();
            return ToJson(filelist);
        }



        [HttpPost, Route("Uploadattachments/{id}")]
        public async Task<HttpResponseMessage> PostFormDataAsync(int id)
        {
            #region "Commented code"
            /*
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Log.Debug(file.Headers.ContentDisposition.FileName);
                    Log.Debug("Server file path: " + file.LocalFileName);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                Log.Debug(e.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

            */
            #endregion

            var path = Path.GetTempPath();

            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }

            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);
            try
            {
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    string fileName = "";
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        fileName = Guid.NewGuid().ToString();
                    }
                    fileName = fileData.Headers.ContentDisposition.FileName;
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

                        newFileName = Path.Combine(HostingEnvironment.MapPath(diskFolderPath), fileName);
                    }

                    if (!Directory.Exists(fileInfo.Directory.FullName))
                    {
                        Directory.CreateDirectory(fileInfo.Directory.FullName);
                    }

                    File.Move(fileData.LocalFileName, newFileName);

                    await SaveToDB(newFileName, id);

                }
                return Request.CreateErrorResponse(HttpStatusCode.OK, "1");
            }
            catch (System.Exception e)
            {
                Log.Debug(e.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }


        [HttpGet, Route("GetfileAttachemnet/{id}"), AllowAnonymous]
        public IHttpActionResult GetfileAttachemnet(int id)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            //try
            //{
                var filelist = (TicketDB.FileUploads.Where(t => t.Fileid == id).Select(p => new { p.Fileid, p.Filedata, p.FileName, p.Filetype, p.UploadDate })).FirstOrDefault();

                var memorycontent = new MemoryStream(filelist.Filedata);
                //  string cotenttype = GetfileContenttype(filelist.Filetype);

                string fileres = Encoding.UTF8.GetString(filelist.Filedata, 0, filelist.Filedata.Length);
                StreamContent  _rescontent= new StreamContent(memorycontent); ;// new StringContent(JsonConvert.SerializeObject(fileres), Encoding.UTF8, cotenttype);
                return new downloadResult(memorycontent, Request, filelist.FileName);
                /*
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(cotenttype);
                response.Content.Headers.Add("x-FileName", filelist.FileName);
                var newFileName = Path.Combine(diskFolderPath, filelist.FileName);
                File.WriteAllBytes(newFileName, filelist.Filedata);
                return response;
                */
            //}
            //catch (System.Exception e)
            //{
            //    Log.Debug(e.Message);
            //    return new HttpActionResult(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);)
            //}
        }

        private async Task<int> SaveToDB(string filename, int ticketid)
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
                TicketDB.FileUploads.Add(fileupload);
                TicketDB.SaveChanges();

                if ((System.IO.File.Exists(filename)))
                {
                    System.IO.File.Delete(filename);
                }

                return 1;
            }
            catch (System.Exception e)
            {
                Log.Debug(e.Message);
                return 0;
            }
        }


        #endregion
    }
}
