using System;
using System.Collections.Generic;
using System.DTO.SubjectDTO;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentSubjectDTOs
{
    public class StudentSubjectDTO
    {
        public int Id { get; set; }
        public string SSN { get; set; }
        public string StudentName { get; set; }
        public double? PracticalDegree { get; set; }
        public double? TheoreticlaDegree { get; set; }
        public int? ExamSemesterId { get; set; }
        public bool HelpDegree { get; set; }
        public string Note { get; set; }
        public string SystemNote { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        
        public ResponseSubjectDTO Subject { get; set; }
        public int ExamSemesterNumber { get; set; }
        public int FirstYear { get; set; }
        public bool Updateable { get; set; }
        
    }
}
