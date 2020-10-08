using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.College
{
    public class UpdateCollegeDTO
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int ProvinceId { get; set; }
    }
}
