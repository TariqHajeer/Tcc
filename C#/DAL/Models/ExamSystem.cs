using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ExamSystem
    {
        public ExamSystem()
        {
            Years = new HashSet<Years>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool HaveTheredSemester { get; set; }
        public bool IsDoubleExam { get; set; }
        public int? GraduateStudentsSemester { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Years> Years { get; set; }
    }
}
