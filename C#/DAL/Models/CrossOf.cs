using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class CrossOf
    {
        public string Ssn { get; set; }
        public int RequserNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Students SsnNavigation { get; set; }
    }
}
