using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class MoreInformation
    {
        public string Ssn { get; set; }
        public string StudentWork { get; set; }
        public string FatherWork { get; set; }
        public DateTime? FatherBirthDay { get; set; }
        public int? FatherAge { get; set; }
        public int? FatherIncome { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public DateTime? MotherBirthDay { get; set; }
        public int? MotherAge { get; set; }
        public string MotherWork { get; set; }
        public string TemporaryAddress { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }

        public Students SsnNavigation { get; set; }
    }
}
