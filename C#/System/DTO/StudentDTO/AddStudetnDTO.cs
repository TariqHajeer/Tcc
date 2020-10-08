using DAL.infrastructure;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentDTO
{
    public class AddStudetnDTO:ISSN
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
        /// <Note>
        /// Cehck it  Form Collages Table
        /// </Note>
        public int? TransformedFromId { get; set; }
        public string SpecializationId { get; set; }
        /// <Note>
        /// this for first time for add student 
        /// add put in Registrations object  
        /// </Note>
        public int TypeOfRegistarId { get; set; }


        public AddMoreInformationDTO MoreInformation { get; set; }
        public AddStudentDegreeDTO StudentDegree { set; get; }

        public List<AddClosestPerson> ClosesetPersons { get; set; }
        public List<AddPartnersDTO> Partners { get; set; }
        public AddRegistrationDTO AddRegistrationDTO { set; get; }

        public List<AddSbilingsDTO> Siblings { get; set; }
        public List<AddStudentPhoneDTO> StudentPhone { get; set; }
    }
    public class AddRegistrationDTO
    {
        public string Ssn { get; set; }
        // ما بدك ياه للطالب الجديد
        public int ? StudyYearId { get; set; }
        public int YearId { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardDate { get; set; }
        // ما بدك ياه للطالب الجديد
        public int ? StudentStateId { get; set; }
        public DateTime? SoldierDate { get; set; }
        public string Note { get; set; }
    }
    public class AddStudentAttachment
    {
        public string Ssn { get; set; }
        public int AttachmentId { get; set; }
        public IFormFile Attachemnt { get; set; }
        public string Note { get; set; }

    }

    public class AddSbilingsDTO
    {
        public string Ssn { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Work { get; set; }
        public int? SocialState { get; set; }
    }
    public class AddStudentPhoneDTO
    {
        public string Ssn { get; set; }
        public int PhoneTypeId { get; set; }
        public string Phone { get; set; }
    }
    public class AddPartnersDTO
    {
        public string Ssn { get; set; }
        public string Name { get; set; }
        public int NationaliryId { get; set; }
        public string PartnerWork { get; set; }
    }
    public class AddClosestPerson
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Ssn { get; set; }
        public List<AddPersonPhone> PersonPhone { get; set; }
    }
    public class AddPersonPhone
    {
        public int PersonId { get; set; }
        public int PhoneTypeId { get; set; }
        public string Phone { get; set; }
    }

    public class AddStudentDegreeDTO
    {
        public int DegreeId { get; set; }
        public string Ssn { get; set; }
        public double Degree { get; set; }
        public int Source { get; set; }
        public int Date { get; set; }
    }
    public class AddMoreInformationDTO
    {
        public string Ssn { get; set; }
        public string StudentWork { get; set; }
        public string FatherWork { get; set; }
        public DateTime? FatherBirthDay { get; set; }
        public int? FatherAge { get; set; }
        public int? FatherIncome { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public DateTime? MotherBirthDay { get; set; }
        public int? MotherAge { get; set; }
        public string MotherWork { get; set; }
        public string TemporaryAddress { get; set; }
    }
}
