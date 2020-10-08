using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.StudentDTO
{
    public class AddSbiling
    {
        public string Ssn { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Work { get; set; }
        public int? SocialState { get; set; }
    }
}
