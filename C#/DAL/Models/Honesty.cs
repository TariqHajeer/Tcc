using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Honesty
    {
        public Honesty()
        {
            Constraints = new HashSet<Constraints>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int CountryId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Countries Country { get; set; }
        public ICollection<Constraints> Constraints { get; set; }
    }
}
