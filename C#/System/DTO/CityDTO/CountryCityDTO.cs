using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.CityDTO
{
    public class CountryCityDTO
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public static implicit operator CountryCityDTO(Countries c)
        {
            
            return new CountryCityDTO()
            {
                Id = c.Id,
                Name = c.Name   
            };
        }
    }
}
