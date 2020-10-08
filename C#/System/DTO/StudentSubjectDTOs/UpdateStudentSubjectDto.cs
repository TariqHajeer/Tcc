using Static;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentSubjectDTOs
{
    public class UpdateStudentSubjectDto
    {
        public int Id { get; set; }
        public double? PracticalDegree { get; set; }
        public double? TheoreticlaDegree { get; set; }
        public bool HelpDegree { get; set; }
        public string Note { get; set; }
    }
}
