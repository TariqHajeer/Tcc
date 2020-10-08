using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Settings
    {
        public Settings()
        {
            SettingYearSystem = new HashSet<SettingYearSystem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<SettingYearSystem> SettingYearSystem { get; set; }
    }
}
