using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.GroupDTO
{
    public class UserGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsEnabled { get; set; }
        public static implicit operator UserGroupDTO(User user)
           => new UserGroupDTO() { Id = user.Id, Name = user.Name, Username = user.Username, IsEnabled = user.IsEnabled };
    }
}
