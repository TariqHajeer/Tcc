using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Siblings
    {
        public int Id { get; set; }
        public string Ssn { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Work { get; set; }
        public int? SocialState { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public SocialStates SocialStateNavigation { get; set; }
        public Students SsnNavigation { get; set; }
    }
}
