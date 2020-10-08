using System;
using System.Collections.Generic;
using System.DTO.StudyPlanDTO;
using System.DTO.SubjectTypeDTO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace System.DTO.SubjectDTO
{
    public class ResponseSubjectDTO
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public int? PracticalTime { get; set; }
        public int? TheoreticalTime { get; set; }
        public int? MainSemesterNumber { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public SubjectTypeResponseDTO SubjectType { set; get; }
        /// <summary>
        /// المواد يلي بعتمد عليها 
        /// </summary>
        public IEnumerable<ResponseSubjectDTO> DependOnSubjects { set; get; }
        /// <summary>
        /// المواد يلي بتعتمد عليي
        /// </summary>
        public IEnumerable<ResponseSubjectDTO> SubjectsDependOnMe { set; get; }
        public IEnumerable<EquivalentSubjectDTO> EqvuvalentSubject { get; set; }
        /// <summary>
        /// هاد الحكي لازمو comment
        /// </summary>
        /// <param name="subject"></param>
        public static implicit operator ResponseSubjectDTO(Subjects subject)
        {
            return new ResponseSubjectDTO
            {
                Id = subject.Id,
                SubjectCode = subject.SubjectCode,
                Name = subject.Name,
                Created = subject.Created,
                CreatedBy = subject.CreatedBy,
                Modified = subject.Modified,
                ModifiedBy = subject.ModifiedBy

            };
        }
    }
    public class EquivalentSubjectDTO
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public SubjectTypeResponseDTO SubjectType { set; get; }
        public string SemesterNumber { get; set; }
        public string StudyYearName { get; set; }
        public string SpecializationName { get; set; }
        public int FirstYear { get; set; }
        public int SecondYear { set; get; }
    }
}
