using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Subjects
    {
        public Subjects()
        {
            DependenceSubjectDependsOnSubject = new HashSet<DependenceSubject>();
            DependenceSubjectSubject = new HashSet<DependenceSubject>();
            EquivalentSubjectFirstSubjectNavigation = new HashSet<EquivalentSubject>();
            EquivalentSubjectSecoundSubjectNavigation = new HashSet<EquivalentSubject>();
            StudentSubject = new HashSet<StudentSubject>();
        }

        public int Id { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public int? PracticalTime { get; set; }
        public int? TheoreticalTime { get; set; }
        public int SubjectTypeId { get; set; }
        public int StudySemesterId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public StudySemester StudySemester { get; set; }
        public SubjectType SubjectType { get; set; }
        public ICollection<DependenceSubject> DependenceSubjectDependsOnSubject { get; set; }
        public ICollection<DependenceSubject> DependenceSubjectSubject { get; set; }
        public ICollection<EquivalentSubject> EquivalentSubjectFirstSubjectNavigation { get; set; }
        public ICollection<EquivalentSubject> EquivalentSubjectSecoundSubjectNavigation { get; set; }
        public ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
