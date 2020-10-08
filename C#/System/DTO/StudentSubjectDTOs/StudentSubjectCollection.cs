using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentSubjectDTOs
{
    public class YearSemeserCollection
    {
        public int FirstYear { get; set; }
        public List<SemesterSubjectCollection> SemesterSubjectCollections { set; get; }
    }
    public class SemesterSubjectCollection
    {
        public int SemesterNumber { get; set; }
        public List<StudentSubjectDTO> Subjects { get; set; }
    }
}
