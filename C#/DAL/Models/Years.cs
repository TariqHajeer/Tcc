using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Years
    {
        public Years()
        {
            ExamSemester = new HashSet<ExamSemester>();
            Registrations = new HashSet<Registrations>();
            StudentDegree = new HashSet<StudentDegree>();
            StudyPlan = new HashSet<StudyPlan>();
        }

        public int Id { get; set; }
        public int FirstYear { get; set; }
        public int SecondYear { get; set; }
        public int YearSystem { get; set; }
        public int ExamSystem { get; set; }
        public bool Blocked { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ExamSystem ExamSystemNavigation { get; set; }
        public YearSystem YearSystemNavigation { get; set; }
        public ICollection<ExamSemester> ExamSemester { get; set; }
        public ICollection<Registrations> Registrations { get; set; }
        public ICollection<StudentDegree> StudentDegree { get; set; }
        public ICollection<StudyPlan> StudyPlan { get; set; }
    }
}
