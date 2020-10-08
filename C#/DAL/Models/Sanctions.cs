using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Sanctions
    {
        public int Id { get; set; }
        public string Sanction { get; set; }
        public string Ssn { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Students SsnNavigation { get; set; }
    }
}
