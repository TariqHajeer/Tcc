using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class SocialStates
    {
        public SocialStates()
        {
            Siblings = new HashSet<Siblings>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Siblings> Siblings { get; set; }
    }
}
