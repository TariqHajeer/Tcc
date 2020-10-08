using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class StudyPlan
    {
        public StudyPlan()
        {
            StudySemester = new HashSet<StudySemester>();
        }

        public int Id { get; set; }
        public int YearId { get; set; }
        public string SpecializationId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Specializations Specialization { get; set; }
        public Years Year { get; set; }
        public ICollection<StudySemester> StudySemester { get; set; }
    }
}
