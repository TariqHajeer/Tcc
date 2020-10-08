using DAL.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Static;
namespace System.DTO.GroupDTO
{
    public class AddGroupDTO
    {
        [Required]
        public string Name { set; get; }
        public List<int> Priveleges { set; get; }
    }

}
