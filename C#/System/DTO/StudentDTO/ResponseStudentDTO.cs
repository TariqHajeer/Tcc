using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentDTO
{
    public class ResponseStudentDTO
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
        public AddMoreInformationDTO MoreInformation { get; set; }
        public StudentDegreeDTO StudentDegree { set; get; }
        public List<ResponseClosesetPersons> ClosesetPersons { get; set; }
        public List<PartnerDTO> Partners { get; set; }
        public List<ReparationDTO> Reparations { get; set; }
        public List<SanctionDTO> Sanctions { get; set; }
        public List<SiblingDTO> Siblings { get; set; }
        public List<StudentAttachmentDTO> StudentAttachment { get; set; }
        public List<StudentPhoneDTO> StudentPhone { get; set; }
        public List<RegistrationDTO> Registrations { get; set; }
    }
    public class RegistrationDTO
    {
        public int Id { get; set; }
        public string Ssn { get; set; }
        public int YearId { get; set; }
        public int TypeOfRegistarId { get; set; }
        public int StudyYearId { get; set; }
        public int ActuallyStudyYear { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardDate { get; set; }
        public int StudentStateId { get; set; }
        public DateTime? SoldierDate { get; set; }
        public string Note { get; set; }
        public string SystemNote { get; set; }
        public int? FinalStateId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class StudentDegreeDTO
    {
        public int DegreeId { get; set; }
        public string Ssn { get; set; }
        public double Degree { get; set; }
        public int Source { get; set; }
        public int Date { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class ResponseClosesetPersons
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<PersonPhoneDTO> PersonPhone { set; get; }

    }
    public class PersonPhoneDTO
    {
        public int PersonId { get; set; }
        public int PhoneTypeId { get; set; }
        public string Phone { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class PartnerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NationaliryId { get; set; }
        public string PartnerWork { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class ReparationDTO
    {
        public int Id { get; set; }
        public string Reparation { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class SanctionDTO
    {
        public int Id { get; set; }
        public string Sanction { get; set; }
        public string Ssn { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class SiblingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Work { get; set; }
        public int? SocialState { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class StudentAttachmentDTO
    {
        public int AttachmentId { get; set; }
        public string Attachemnt { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int Id { get; set; }
    }
    public class StudentPhoneDTO
    {
        public int Id { get; set; }
        public string Ssn { get; set; }
        public int PhoneTypeId { get; set; }
        public string Phone { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
