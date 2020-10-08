using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace DAL.infrastructure
{
   public interface IRepositroy<TEntity> where TEntity :class,IEntity
    {
        TEntity Find(object Id);
        //Task<TEntity> FindTask(object Id);

        //TEntity Find(object[] Id);
        //Task<TEntity> FindTask(object[] Id);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> Fillter = null,params Expression<Func<TEntity,object>>[] propertySelectors);
        //Task<List<TEntity>> GetTask(Expression<Func<TEntity, bool>> Fillter = null, params Expression<Func<TEntity, object>>[] propertySelectors); 
        IQueryable<TEntity> GetIQueryable(Expression<Func<TEntity, bool>> Fillter = null);
        //Task<IQueryable<TEntity>> GetTaskIQueryable(Expression<Func<TEntity, bool>> Fillter = null);

        void Add(TEntity entity, string createdBy);
        //Task<TEntity> UpdateTask(TEntity entity, string modifiedBy);
        TEntity Update(TEntity entity, string modifiedBy);
        bool Remove(TEntity entity, string RemovedBy);
        //Task<bool> RemoveTask(TEntity entity, string RemovedBy);
        void Update(string procName, object[] parameters, TEntity entity, string modifiedBy);
        void Save();
    }
}
