using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TypeOfRegistar
    {
        public TypeOfRegistar()
        {
            Registrations = new HashSet<Registrations>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Registrations> Registrations { get; set; }
    }
}
