using DAL.Models;
using System;
using System.Collections.Generic;
using System.DTO.CityDTO;
using System.Linq;
using System.Threading.Tasks;
using System.DTO.CountryDTO;
namespace System.DTO.CountryDTO
{
    public class CityResponseDTO:CountryResponseDTO
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        public int MainCountry { get; set; }
        //public bool IsEnabled { get; set; }
        //public DateTime Created { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? Modified { get; set; }
        //public string ModifiedBy { get; set; }
        public CountryCityDTO Country { get; set; }
    }
}
