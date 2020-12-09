using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //Create
        TEntity Add(TEntity entity);
        IQueryable<TEntity> AddRange(IEnumerable<TEntity> entities);
        T Add<T>(T entity) where T : class;
        //Read
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null);
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class;
        //Update
        TEntity Edit(TEntity entity);
        T Edit<T>(T entity) where T : class;
        //Delete
        TEntity Remove(TEntity entity);
        TEntity RemoveRange(IEnumerable<TEntity> entities);
        T ExecuteSql<T>(string query, params object[] parameters) where T : class;

        void ExecuteSql(string query);
    }
}
