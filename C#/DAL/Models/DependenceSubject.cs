using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class DependenceSubject
    {
        public int SubjectId { get; set; }
        public int DependsOnSubjectId { get; set; }
        public bool IsEnabeld { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Subjects DependsOnSubject { get; set; }
        public Subjects Subject { get; set; }
    }
}
