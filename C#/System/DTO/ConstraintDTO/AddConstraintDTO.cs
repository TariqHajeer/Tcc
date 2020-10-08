using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.ConstraintDTO
{
    public class AddConstraintDTO
    {
        public string Name { get; set; }
        public int HonestyId { get; set; }
        public bool IsEnabled { get; set; }     
    }
}
