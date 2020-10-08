using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Specializations
    {
        public Specializations()
        {
            Students = new HashSet<Students>();
            StudyPlan = new HashSet<StudyPlan>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Students> Students { get; set; }
        public ICollection<StudyPlan> StudyPlan { get; set; }
    }
}
