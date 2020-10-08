using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DTO.StudentSubjectDTOs;
namespace System.DTO.StudentDTO
{
    public class ResponseStudentHelpDegreeDto
    {
        public IList<StudentSubjectDTO>  StudentSubjects { get; set; }
        public int StillToSucess { get; set; }
        public int HelpDgreeCount { get; set; }
        public int HelpDegreeDivideOn { get; set; }
    }
}
