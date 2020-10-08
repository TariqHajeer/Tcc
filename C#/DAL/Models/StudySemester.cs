using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class StudySemester
    {
        public StudySemester()
        {
            Subjects = new HashSet<Subjects>();
        }

        public int Id { get; set; }
        public short Number { get; set; }
        public int StudyplanId { get; set; }
        public int StudyYearId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public StudyYear StudyYear { get; set; }
        public StudyPlan Studyplan { get; set; }
        public ICollection<Subjects> Subjects { get; set; }
    }
}
