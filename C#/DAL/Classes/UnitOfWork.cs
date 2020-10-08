using DAL.infrastructure;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace DAL.Classes
{
    public class UnitOfWork : AbstractUnitOfWork
    {


        public UnitOfWork(DbContext tccContext) : base(tccContext)
        {
            this._context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public override void Commit()
            => _context.SaveChanges();

        /// <summary>
        /// IDisposable
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// custom dispose
        /// </summary>
        /// <param name="disposing"></param>

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }



        public override Repository<TEntity> Repository<TEntity>()
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<TEntity>);
                var repositoryInstance = new Repository<TEntity>(_context);
                //var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType());
                _repositories.Add(type, repositoryInstance);
            }
            return (Repository<TEntity>)_repositories[type];
        }

        public override void Add<TEntity>(TEntity entity, string createdBy)
        {
            Repository<TEntity>().Add(entity, createdBy);
        }

        public override void BegeinTransaction()
        {
            transaction = Context.Database.BeginTransaction();
        }

        public override void RoleBack()
        {
            transaction.Rollback();
            transaction.Dispose();
            transaction = null;
        }

        public override void CommitTransaction()
        {
            transaction.Commit();
        }

        public override TEntity FirstOrDefult<TEntity>()
        {
            return this.Repository<TEntity>().Get().FirstOrDefault();
        }

        public override void Update<TEntity>(TEntity entity, string updatedBy)
        {
            Repository<TEntity>().Update(entity, updatedBy);
        }

        public override void Remove<TEntity>(TEntity entity, string RemovedBy)
        {
            Repository<TEntity>().Remove(entity, RemovedBy);
        }

        //public override void Update<TEntity>(string procName, object[] parameters, TEntity entity, string updatedBy)
        //{
        //    Repository<TEntity>().Updated(procName, parameters, entity, updatedBy);
        //}
    }
}
