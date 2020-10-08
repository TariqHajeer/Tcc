using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Colleges
    {
        public Colleges()
        {
            Students = new HashSet<Students>();
            Trasmentd = new HashSet<Trasmentd>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Countries Province { get; set; }
        public ICollection<Students> Students { get; set; }
        public ICollection<Trasmentd> Trasmentd { get; set; }
    }
}
