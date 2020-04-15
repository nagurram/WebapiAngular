using System;

namespace LSDataApi.DBContext
{
    public partial class TodoList
    {
        public int TodoId { get; set; }
        public string Titile { get; set; }
        public string Description { get; set; }
        public DateTime? ActionDate { get; set; }
        public bool? IsActive { get; set; }
        public int? Userid { get; set; }
    }
}