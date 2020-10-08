using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class StudentDegree
    {
        public int DegreeId { get; set; }
        public string Ssn { get; set; }
        public double Degree { get; set; }
        public int Source { get; set; }
        public int Date { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Years DateNavigation { get; set; }
        public Degree DegreeNavigation { get; set; }
        public Countries SourceNavigation { get; set; }
        public Students SsnNavigation { get; set; }
    }
}
