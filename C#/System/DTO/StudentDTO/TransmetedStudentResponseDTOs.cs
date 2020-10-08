using System;
using System.Collections.Generic;
using System.DTO.StudentSubjectDTOs;
using System.DTO.SubjectDTO;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentDTO
{
    public class TransmetedStudentResponseDTO
    {
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
        public List<StudentSubjectDTO> Subjects { get; set; }
    }
    //public class StudentSubjectDTO
    //{
    //    public int Id { get; set; }
    //    public string SSN { get; set; }
    //    public string StudentName { get; set; }
    //    public double? PracticalDegree { get; set; }
    //    public double? TheoreticlaDegree { get; set; }
    //    public int? ExamSemesterId { get; set; }
    //    public string Note { get; set; }
    //    public string SystemNote { get; set; }
    //    public DateTime Created { get; set; }
    //    public string CreatedBy { get; set; }
    //    public DateTime? Modified { get; set;}
    //    public string ModifiedBy { get; set; }
    //    public double HelperDegree { get; set; }
    //    public ResponseSubjectDTO Subject { get; set; }
    //}   
}
