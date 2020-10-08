using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.ConstraintDTO
{
    public class ConstraintResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HonestyId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public HonestyConstraintDTO Honesty  { get; set; }

    }
}
