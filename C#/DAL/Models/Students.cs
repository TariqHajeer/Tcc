using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Students
    {
        public Students()
        {
            ClosesetPersons = new HashSet<ClosesetPersons>();
            Partners = new HashSet<Partners>();
            Registrations = new HashSet<Registrations>();
            Reparations = new HashSet<Reparations>();
            Sanctions = new HashSet<Sanctions>();
            Siblings = new HashSet<Siblings>();
            StudentAttachment = new HashSet<StudentAttachment>();
            StudentPhone = new HashSet<StudentPhone>();
            StudentSubject = new HashSet<StudentSubject>();
        }

        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthBlace { get; set; }
        public int? ConstraintId { get; set; }
        public short? ConstraintNumber { get; set; }
        public int NationalityId { get; set; }
        public int ProvinceId { get; set; }
        public bool Sex { get; set; }
        public string PermanentAddress { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int LanguageId { get; set; }
        public int? TransformedFromId { get; set; }
        public string SpecializationId { get; set; }
        public DateTime? CeasedFromTheCollage { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Constraints Constraint { get; set; }
        public Langaues Language { get; set; }
        public Nationalies Nationality { get; set; }
        public Countries Province { get; set; }
        public Specializations Specialization { get; set; }
        public Colleges TransformedFrom { get; set; }
        public CrossOf CrossOf { get; set; }
        public Deattach Deattach { get; set; }
        public Graduation Graduation { get; set; }
        public MoreInformation MoreInformation { get; set; }
        public StudentDegree StudentDegree { get; set; }
        public Trasmentd Trasmentd { get; set; }
        public ICollection<ClosesetPersons> ClosesetPersons { get; set; }
        public ICollection<Partners> Partners { get; set; }
        public ICollection<Registrations> Registrations { get; set; }
        public ICollection<Reparations> Reparations { get; set; }
        public ICollection<Sanctions> Sanctions { get; set; }
        public ICollection<Siblings> Siblings { get; set; }
        public ICollection<StudentAttachment> StudentAttachment { get; set; }
        public ICollection<StudentPhone> StudentPhone { get; set; }
        public ICollection<StudentSubject> StudentSubject { get; set; }
    }
}
