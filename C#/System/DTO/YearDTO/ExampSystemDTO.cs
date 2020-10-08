using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
namespace System.DTO.YearDTO
{
    public class ExampSystemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static implicit operator ExampSystemDTO(ExamSystem examSystem)
        {
            if (examSystem == null)
                return null;
            return new ExampSystemDTO()
            {
                Id = examSystem.Id,
                Name = examSystem.Name
            };
        }
    }
}
