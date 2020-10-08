using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class YearSystem
    {
        public YearSystem()
        {
            SettingYearSystem = new HashSet<SettingYearSystem>();
            Years = new HashSet<Years>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsMain { get; set; }

        public ICollection<SettingYearSystem> SettingYearSystem { get; set; }
        public ICollection<Years> Years { get; set; }
    }
}
