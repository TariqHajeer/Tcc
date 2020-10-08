using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.ExamSystemDTO
{
    public class ResponseExamSystemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? HaveTheredSemester { get; set; }
        public bool? IsDoubleExam { get; set; }
        public int? GraduateStudentsSemester { get; set; }
        public bool IsEnabled { get; set; }
        public bool Updateable { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
