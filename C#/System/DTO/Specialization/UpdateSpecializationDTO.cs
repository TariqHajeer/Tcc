using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.Specialization
{
    public class UpdateSpecializationDTO
    {
        public string Id { get; set; }
        public string  Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
