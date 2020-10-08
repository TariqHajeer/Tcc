using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.UserDTO
{
    public class AuthUserDTO
    {
        public string Username { set; get; }
        public string Token { set; get; }
        public List<string> Roles { set; get; }
        public string MainPage { set; get; }
    }
}
