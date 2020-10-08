using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
namespace System.DTO.LanguageDTO
{
    public class AddLanguageDTO
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
