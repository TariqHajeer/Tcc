using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.SubjectDTO
{
    public class AddSubjectDTO
    {
        public int TempId { set; get; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public int SubjectTypeId { get; set; }
        public int StudySemesterId { get; set; }
        public int? PracticalTime { get; set; }
        public int? TheoreticalTime { get; set; }
        public List<int> DependencySubjectsId { set; get; }
        public List<int> EquivalentSubjectsId { set; get; }
    }
}
