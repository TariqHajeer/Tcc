using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.SubjectTypeDTO
{
    public class SubjectTypeResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int PracticalDegree { get; set; }
        public int NominateDegree { get; set; }
        public int TheoreticalDegree { get; set; }
        public int SuccessDegree { get; set; }
        public string SpecializationId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
