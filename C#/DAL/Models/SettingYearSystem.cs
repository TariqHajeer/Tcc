using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class SettingYearSystem
    {
        public int YearSystem { get; set; }
        public int SettingId { get; set; }
        public int Count { get; set; }
        public DateTime Created { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Settings Setting { get; set; }
        public YearSystem YearSystemNavigation { get; set; }
    }
}
