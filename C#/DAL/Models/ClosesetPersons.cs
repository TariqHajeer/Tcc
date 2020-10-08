using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ClosesetPersons
    {
        public ClosesetPersons()
        {
            PersonPhone = new HashSet<PersonPhone>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Ssn { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Students SsnNavigation { get; set; }
        public ICollection<PersonPhone> PersonPhone { get; set; }
    }
}
