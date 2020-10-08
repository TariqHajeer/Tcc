using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Partners
    {
        public int Id { get; set; }
        public string Ssn { get; set; }
        public string Name { get; set; }
        public int NationaliryId { get; set; }
        public string PartnerWork { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Nationalies Nationaliry { get; set; }
        public Students SsnNavigation { get; set; }
    }
}
