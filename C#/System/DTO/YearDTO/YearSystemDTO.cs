using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
namespace System.DTO.YearDTO
{
    public class YearSystemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public static implicit operator YearSystemDTO(YearSystem yearSystem)
        {
            if (yearSystem == null)
                return null;
            return new YearSystemDTO()
            {
                Id = yearSystem.Id,
                Name = yearSystem.Name,
                Note = yearSystem.Note,
            };
        }
    }
}
