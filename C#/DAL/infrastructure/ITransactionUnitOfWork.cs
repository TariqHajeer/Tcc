using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.infrastructure
{
    interface ITransactionUnitOfWork:IUnitOfWork
    {
        void BegeinTransaction();
        
        void RoleBack();
        void CommitTransaction();
    }
}
