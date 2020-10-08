using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.UserDTO
{
    public class UpdateUserDTO
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsEnabled { get; set; }
    }
}
