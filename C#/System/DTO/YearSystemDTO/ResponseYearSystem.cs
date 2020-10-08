using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.YearSystemDTO
{
    public class ResponseYearSystem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsMain { get; set; }
        public bool Updateable { get; set; }
        public List<SettingYearSystemDTO> Settings { set; get; }
    }
}
