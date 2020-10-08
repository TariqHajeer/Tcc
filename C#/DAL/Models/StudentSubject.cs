using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class StudentSubject
    {
        public int Id { get; set; }
        public string Ssn { get; set; }
        public int SubjectId { get; set; }
        public double? PracticalDegree { get; set; }
        public double? TheoreticlaDegree { get; set; }
        public int? ExamSemesterId { get; set; }
        public string Note { get; set; }
        public string SystemNote { get; set; }
        public bool? HelpDegree { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ExamSemester ExamSemester { get; set; }
        public Students SsnNavigation { get; set; }
        public Subjects Subject { get; set; }
    }
}
