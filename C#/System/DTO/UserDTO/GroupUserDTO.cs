using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.UserDTO
{
    public class GroupUserDTO
    {
        public int Id { set; get; }
        public string Name { set; get; }
        //public List<PrivilegeUserDTO> Privilage { set; get; }
        public static implicit operator GroupUserDTO(Group group)
        {
            return new GroupUserDTO()
            {
                Id = group.Id,
                Name = group.Name,
                //Privilage = group.GroupPrivilage.Select(c => c.Privilage).ToList(),
            };
        }
    }
}
