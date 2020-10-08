using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupPrivilage = new HashSet<GroupPrivilage>();
            UserGroup = new HashSet<UserGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<GroupPrivilage> GroupPrivilage { get; set; }
        public ICollection<UserGroup> UserGroup { get; set; }
    }
}
