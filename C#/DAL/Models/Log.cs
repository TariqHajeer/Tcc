using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string BeforAction { get; set; }
        public string AfterAction { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
    }
}
