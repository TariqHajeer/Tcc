using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.RegistrationDTOs
{
    public class CreateRegistrationDTO
    {
        public int TypeOfRegistarId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? CardDate { get; set; }
        public DateTime? SoldierDate { get; set; }
        public string Note { get; set; }
    }
}
