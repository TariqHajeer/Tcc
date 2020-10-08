using System;
using System.Collections.Generic;
using System.Text;
using DAL.Classes;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
namespace DAL.infrastructure
{
    interface IUnitOfWork:IDisposable
    {
        void Commit();
    }
}
