using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudySemester
{
    public class AddStudySemesterDTO
    {
        public int Number { get; set; }
        public int StudyPlanId { get; set; }
        public int StudyYearId { get; set; }
        public int YearId { get; set; }
    }
}
