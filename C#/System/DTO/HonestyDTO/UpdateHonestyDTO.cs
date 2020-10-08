using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.HonestyDTO
{
    public class UpdateHonestyDTO
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int CountryId { get; set; }
    }
}
