using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.ExamSystemDTO
{
    public class UpdateExamSystemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? HaveTheredSemester { get; set; }
        public bool? IsDoubleExam { get; set; }
        public bool IsEnabled { get; set; }
    }
}
