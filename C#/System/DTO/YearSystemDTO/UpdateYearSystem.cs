using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.YearSystemDTO
{
    public class UpdateYearSystem
    {
        public string Name{ get; set; }
        public string Note { get; set; }
        public bool IsMain { get; set; }
    }
}
