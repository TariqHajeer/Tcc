using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DTO.CollegesDTO;

namespace System.DTO.College
{
    public class CollegesResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public CityCollageDTO City { set; get; }
        
    }
}
