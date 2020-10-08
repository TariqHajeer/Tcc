using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DTO.GroupDTO
{
    public class GroupResopnseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int PrivilagesCount { set; get; }
        public int UserCount { set; get; }
        public List<PrivelegGroupDTO> Privilages { set; get; }
        public List<UserGroupDTO> User { get; set; }

    }
}
