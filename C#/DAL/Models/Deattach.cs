using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Deattach
    {
        public string Ssn { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Decision { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Students SsnNavigation { get; set; }
    }
}
