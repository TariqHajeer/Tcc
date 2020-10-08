using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ExamSemester
    {
        public ExamSemester()
        {
            StudentSubject = new HashSet<StudentSubject>();
        }

        public int Id { get; set; }
        public int SemesterNumber { get; set; }
        public int YearId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Years Year { get; set; }
        public ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
