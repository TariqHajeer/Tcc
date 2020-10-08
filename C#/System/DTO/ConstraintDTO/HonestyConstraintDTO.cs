using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.ConstraintDTO
{
    public class HonestyConstraintDTO
    {

        public int Id { set; get; }
        public string Name { set; get; }
        public static implicit operator HonestyConstraintDTO(Honesty c)
        {
            return new HonestyConstraintDTO()
            {
                Id = c.Id,
                Name = c.Name
            };
        }
    }
}
