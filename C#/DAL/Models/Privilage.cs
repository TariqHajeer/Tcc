using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Privilage
    {
        public Privilage()
        {
            GroupPrivilage = new HashSet<GroupPrivilage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }

        public ICollection<GroupPrivilage> GroupPrivilage { get; set; }
    }
}
