using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.YearSystemDTO
{
    public class AddYearSystemDTO
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsMain { get; set; }
        public List<AddYearSettingDTO> Settings { get; set; }
    }
}
