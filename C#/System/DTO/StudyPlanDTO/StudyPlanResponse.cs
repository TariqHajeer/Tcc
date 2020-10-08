using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using System.DTO.SubjectDTO;
using System.DTO.Specialization;
using System.DTO.YearDTO;
using System.DTO.CommonDTO;

namespace System.DTO.StudyPlanDTO
{
    public class StudyPlanResponse
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public SpecialziationResponseDTO Specialization { set; get; }
        public YearResponseDTO Year { set; get; }
        public List<StudySemesterDTO> StudySemester { set; get; }
        public bool Updateable { get; set; }
    }
    public class StudySemesterDTO
    {
        public int Id { get; set; }
        public short Number { get; set; }
        public StudyYearDTO StudyYear { set; get; }
        public List<ResponseSubjectDTO> Subjects { set; get; }
    }
}
