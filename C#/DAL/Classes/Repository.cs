using DAL.infrastructure;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Classes
{
    public class Repository<TEntity> : IRepositroy<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbContext _context;
        public Repository(DbContext tccContext)
        => _context = tccContext;
        public void Add(TEntity entity, string createdBy)
        {
            entity.Created = DateTime.Now;
            entity.CreatedBy = createdBy;
            entity.Modified = null;
            entity.ModifiedBy = string.Empty;

            _context.Entry(entity).State = EntityState.Added;

            var log = new Log()
            {
                EntityName = entity.GetType().Name,
                AfterAction = entity.ToString(),
                Created = DateTime.Now,
                CreatedBy = createdBy,
            };
            _context.Set<TEntity>().Add(entity);
            _context.Add(log);
        }
        public TEntity Find(object Id)
        {
            var entity = _context.Set<TEntity>().Find(Id);
            return entity;
        }


        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> Fillter = null, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var entitySet = _context.Set<TEntity>().AsQueryable();
            if (Fillter != null)
            {
                entitySet = entitySet.Where(Fillter);
            }
            if (propertySelectors != null)
            {
                foreach (var item in propertySelectors)
                {
                    entitySet = entitySet.Include(item);
                }
            }
            return entitySet.ToList();
        }

        public IQueryable<TEntity> GetIQueryable(Expression<Func<TEntity, bool>> Fillter = null)
        {
            if (Fillter == null)
                return _context.Set<TEntity>();
            return _context.Set<TEntity>().Where(Fillter);
        }
        //public IEnumerable<TEntity>Get()


        public bool Remove(TEntity entity, string RemovedBy)
        {
            try
            {
                //_context.Entry(entity).State = EntityState.Deleted;
                _context.Set<TEntity>().Remove(entity);
                //_context.ChangeTracker.Entries();
                //_context.ChangeTracker.DetectChanges();
                //_context.ChangeTracker.AutoDetectChangesEnabled = false;
                Log log = new Log()
                {
                    EntityName = entity.GetType().Name,
                    Created = DateTime.Now,
                    CreatedBy = RemovedBy,
                    BeforAction = entity.ToString(),
                    AfterAction = "Removed"
                };
                _context.Add(log);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public TEntity Update(TEntity entity, string modifiedBy)
        {
            try
            {

                TEntity oldEntity = (TEntity)_context.Entry(entity).GetDatabaseValues().ToObject();
                // for prevent change on created - by

                entity.Created = oldEntity.Created;
                entity.CreatedBy = oldEntity.CreatedBy;


                entity.Modified = DateTime.Now;
                entity.ModifiedBy = modifiedBy;
                _context.Entry(entity).State = EntityState.Modified;
                Log log = new Log()
                {
                    Created = DateTime.Now,
                    CreatedBy = modifiedBy,
                    BeforAction = oldEntity.ToString(),
                    AfterAction = entity.ToString(),
                    EntityName = entity.GetType().Name
                };
                _context.Add(log);
                return entity;
            }
            catch (Exception e)
            {
                //return null;

                throw e;
            }

        }
        public void Update(string ProcExecute, object[] parameters, TEntity entity, string modifiedBy)
        {
            try
            {
                TEntity oldEntity = (TEntity)_context.Entry(entity).GetDatabaseValues().ToObject();
                _context.Database.ExecuteSqlCommand(ProcExecute, parameters: parameters);
                Log log = new Log()
                {
                    Created = DateTime.Now,
                    CreatedBy = modifiedBy,
                    BeforAction = oldEntity.ToString(),
                    AfterAction = entity.ToString(),
                    EntityName = entity.GetType().Name
                };
                _context.Add(log);
            }
            catch (Exception e)
            {
                //return null;

                throw e;
            }

        }
        public void Save()
        {
            _context.SaveChanges();
        }

        //public Task<List<TEntity>> GetTask(Expression<Func<TEntity, bool>> Fillter = null, params Expression<Func<TEntity, object>>[] propertySelectors)
        //{
        //    var entitySet = _context.Set<TEntity>().AsQueryable();
        //    if (Fillter != null)
        //    {
        //        entitySet = entitySet.Where(Fillter);
        //    }
        //    if (propertySelectors != null)
        //    {
        //        foreach (var item in propertySelectors)
        //        {
        //            entitySet = entitySet.Include(item);
        //        }
        //    }
        //    return   entitySet.ToListAsync();
        //}
    }
}
