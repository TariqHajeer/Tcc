using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace System.DTO.StudyPlanDTO
{
    public class AddStudyPalnDTO
    {
        public int YearId { get; set; }
        public string SpecializationId { get; set; }
        public bool IsEnabled { get; set; } 
        public List<StudyPlanAddSubjectDTO> Subjects { set; get; }
    }
}
