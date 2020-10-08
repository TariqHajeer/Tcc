using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Registrations
    {
        public int Id { get; set; }
        public string Ssn { get; set; }
        public int YearId { get; set; }
        public int TypeOfRegistarId { get; set; }
        public int StudyYearId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? CardDate { get; set; }
        public int StudentStateId { get; set; }
        public DateTime? SoldierDate { get; set; }
        public string Note { get; set; }
        public string SystemNote { get; set; }
        public int? FinalStateId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public StudentState FinalState { get; set; }
        public Students SsnNavigation { get; set; }
        public StudentState StudentState { get; set; }
        public StudyYear StudyYear { get; set; }
        public TypeOfRegistar TypeOfRegistar { get; set; }
        public Years Year { get; set; }
    }
}
