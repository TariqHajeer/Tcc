using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class PhoneType
    {
        public PhoneType()
        {
            PersonPhone = new HashSet<PersonPhone>();
            StudentPhone = new HashSet<StudentPhone>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<PersonPhone> PersonPhone { get; set; }
        public ICollection<StudentPhone> StudentPhone { get; set; }
    }
}
