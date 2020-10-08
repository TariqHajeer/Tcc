using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.YearDTO
{
    public class YearResponseDTO
    {
        public int Id { get; set; }
        public int FirstYear { get; set; }
        public int SecondYear { get; set; }
        public bool Blocked { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public ExampSystemDTO ExamSystem { get; set; }
        public YearSystemDTO Yearystem { get; set; }
    }
}
