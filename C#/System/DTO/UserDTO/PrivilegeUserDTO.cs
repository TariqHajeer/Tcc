using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.UserDTO
{
    public class PrivilegeUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator PrivilegeUserDTO(Privilage privilage)
        {
            return new PrivilegeUserDTO() { Id = privilage.Id, Name = privilage.Name, Description = privilage.Description };
        }
    }
}
