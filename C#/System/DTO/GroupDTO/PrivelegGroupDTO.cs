using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.GroupDTO
{
    public class PrivelegGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public static implicit operator PrivelegGroupDTO(Privilage privilage)
        {
            
            return new PrivelegGroupDTO() { Id = privilage.Id, Name = privilage.Name, Description = privilage.Description };
        }
    }
}
