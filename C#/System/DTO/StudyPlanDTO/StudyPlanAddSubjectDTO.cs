using System;
using System.Collections.Generic;
using System.DTO.SubjectDTO;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudyPlanDTO
{
    public class StudyPlanAddSubjectDTO
    {
        public AddSubjectDTO AddSubjectDTO { set; get; }
        public int StudyYearId { set; get; }
        public int StudySemesterNumber { set; get; }
    }
}
