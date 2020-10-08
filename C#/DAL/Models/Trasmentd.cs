using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Trasmentd
    {
        public string Ssn { get; set; }
        public int CollageId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Colleges Collage { get; set; }
        public Students SsnNavigation { get; set; }
    }
}
