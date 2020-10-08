using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Countries
    {
        public Countries()
        {
            Colleges = new HashSet<Colleges>();
            Honesty = new HashSet<Honesty>();
            InverseMainCountryNavigation = new HashSet<Countries>();
            StudentDegree = new HashSet<StudentDegree>();
            Students = new HashSet<Students>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? MainCountry { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Countries MainCountryNavigation { get; set; }
        public ICollection<Colleges> Colleges { get; set; }
        public ICollection<Honesty> Honesty { get; set; }
        public ICollection<Countries> InverseMainCountryNavigation { get; set; }
        public ICollection<StudentDegree> StudentDegree { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}
