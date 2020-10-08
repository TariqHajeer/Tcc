using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class EquivalentSubject
    {
        public int FirstSubject { get; set; }
        public int SecoundSubject { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Subjects FirstSubjectNavigation { get; set; }
        public Subjects SecoundSubjectNavigation { get; set; }
    }
}
