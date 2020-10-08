using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DTO.SubjectDTO;
namespace System.DTO.ExamSemesterDTO
{
    public class ResponseExamSemesterDTO
    {
        public int Id { get; set; }
        public int SemesterNumber { get; set; }
        public int YearId { get; set; }
        public List<ResponseSubjectDTO> Subjects { get; set; }
    }
}
