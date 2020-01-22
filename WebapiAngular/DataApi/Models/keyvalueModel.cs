using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataApi.Models
{
    public class keyvalueModel
    {
        public int Id { get; set; }
        public string keyValue { get; set; }
        public bool? IsDeleted { get; set; }
    }
}