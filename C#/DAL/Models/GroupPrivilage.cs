using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class GroupPrivilage
    {
        public int GroupId { get; set; }
        public int PrivilageId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Group Group { get; set; }
        public Privilage Privilage { get; set; }
    }
}
