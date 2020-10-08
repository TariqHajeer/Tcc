using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentDTO
{
    public class StudentsResponseDTO
    {
        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string CurrentYear { get; set; }
        public string CurrentState { get; set; }
        public int ActuallyStudyYear { get; set; }
        public string FinalState { get; set; }
        public string SpecializationName{ get; set; }
    }
}
