using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
namespace System.DTO.CollegesDTO
{
    public class CityCollageDTO
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public static implicit operator CityCollageDTO(Countries c)
        {
            return new CityCollageDTO()
            {
                Id = c.Id,
                Name = c.Name
            };
        }
    }
}
