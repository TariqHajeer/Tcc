using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.HonestyDTO
{
    public class CountryHonestyDTO
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public static implicit operator CountryHonestyDTO(Countries c)
        {
            return new CountryHonestyDTO()
            {
                Id = c.Id,
                Name = c.Name
            };
        }
    }
}
