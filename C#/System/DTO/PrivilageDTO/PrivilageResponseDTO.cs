using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System.DTO.PrivilageDTO
{
    public class PrivilageResponseDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int groupCount { get; set; }
        public DateTime? Modified { get; set; }
        public List<Group> groups { get; set; }

    }
}
