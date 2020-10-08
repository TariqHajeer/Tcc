using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.CountryDTO
{
    public class AddCityDTO
    {
        public string Name { get; set; }
        public int? MainCountry { get; set; }
        public bool IsEnabled { get; set; }
    }
}
