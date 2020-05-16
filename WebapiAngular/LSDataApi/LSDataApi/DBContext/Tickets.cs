using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class Tickets
    {
        public Tickets()
        {
            FileUpload = new HashSet<FileUpload>();
        }

        public int TicketId { get; set; }
        public string Title { get; set; }
        public string Tdescription { get; set; }
        public int? CreatedBy { get; set; }
        public int? StatusId { get; set; }
        public DateTime? Createddate { get; set; }
        public int? AssignedTo { get; set; }
        public int? PriorityId { get; set; }
        public int? TypeId { get; set; }
        public int? ApplicationId { get; set; }
        public int? ModuleId { get; set; }
        public DateTime? ResponseDeadline { get; set; }
        public DateTime? ResolutionDeadline { get; set; }
        public int? RootCauseId { get; set; }
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? LastModifiedon { get; set; }

        public virtual ApplicationMaster Application { get; set; }
        public virtual ModuleMaster Module { get; set; }
        public virtual PriorityMaster Priority { get; set; }
        public virtual RootCauseMaster RootCause { get; set; }
        public virtual StatusMaster Status { get; set; }
        public virtual TypeMaster Type { get; set; }
        public virtual Resource UpdatedByNavigation { get; set; }
        public virtual ICollection<FileUpload> FileUpload { get; set; }
    }
}
