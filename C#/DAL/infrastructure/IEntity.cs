using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.infrastructure
{
    public interface IEntity
    {
        DateTime Created { get; set; }
        string CreatedBy { get; set; }
        DateTime? Modified { get; set; }
        string ModifiedBy { get; set; }
        string Log();
        
    }
    
}
