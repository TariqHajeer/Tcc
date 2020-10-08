using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentDTO
{
    public class AddPhoneForStudent
    {
        public string Ssn { get; set; }
        public int PhoneTypeId { get; set; }
        public string Phone { get; set; }
    }
}
