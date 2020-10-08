using System;
using System.Collections.Generic;
using System.DTO.StudentSubjectDTOs;
using System.DTO.SubjectDTO;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentDTO
{
    public class UpdateStudnetInformation
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
        public UpdateMoreInformationDto MoreInformation { get; set; }
    }
    public class UpdateMoreInformationDto
    {
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