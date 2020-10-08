using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.UserDTO
{
    public class AddUserDTO
    {
        public string Username { get; set; }
        public string Name { set; get; }
        public string Password { set; get; }
        public bool IsEnabled { set; get; }
        public List<int> GroupIds { set; get; }
    }
}
