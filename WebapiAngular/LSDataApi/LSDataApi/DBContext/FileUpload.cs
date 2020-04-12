using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class FileUpload
    {
        public int Fileid { get; set; }
        public byte[] Filedata { get; set; }
        public int? TicketId { get; set; }
        public string Filetype { get; set; }
        public DateTime? UploadDate { get; set; }
        public string FileName { get; set; }

        public virtual Tickets Ticket { get; set; }
    }
}
