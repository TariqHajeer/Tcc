using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class UserGroup
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }

        public Group Group { get; set; }
        public User User { get; set; }
    }
}
