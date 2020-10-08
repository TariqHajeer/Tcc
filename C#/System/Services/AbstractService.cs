using DAL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Services
{
    abstract public class AbstractService
    {
        protected AbstractUnitOfWork _abstractUnitOfWork;
        public AbstractService(AbstractUnitOfWork abstractUnitOfWork) => _abstractUnitOfWork = abstractUnitOfWork;

    }
}
