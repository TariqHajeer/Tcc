using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Graduation
    {
        public string Ssn { get; set; }
        public int GraduationDesicion { get; set; }
        public DateTime DesicionDate { get; set; }
        public int? CollageDesicion { get; set; }
        public DateTime? Cddate { get; set; }
        public int? MinistryApproval { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string GeneralAppreciation { get; set; }
        public decimal? GeneralAverage { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Students SsnNavigation { get; set; }
    }
}
