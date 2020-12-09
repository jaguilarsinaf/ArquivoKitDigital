using Projeto.Domain.Interfaces.Repositories;
using Projeto.Infra.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {

        private readonly Conexao _db = new Conexao();
        public Conexao getContext()
        {
            return _db;
        }

        public TEntity Add(TEntity entity)
        {
            try
            {
                _db.Set<TEntity>().Add(entity);
                _db.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public T Add<T>(T entity) where T : class
        {
            try
            {
                _db.Set<T>().Add(entity);
                _db.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _db.Set<TEntity>().AddRange(entities);
                _db.SaveChanges();
                return entities.AsQueryable();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Dispose()
        {
            if (_db != null)
                _db.Dispose();
        }

        public TEntity Edit(TEntity entity)
        {
            try
            {
                _db.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public T Edit<T>(T entity) where T : class
        {
            try
            {
                _db.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                if (predicate == null)
                    return _db.Set<TEntity>().AsQueryable();
                return _db.Set<TEntity>().Where(predicate).AsQueryable();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            try
            {
                if (predicate == null)
                    return _db.Set<T>().AsQueryable();

                return _db.Set<T>().Where(predicate).AsQueryable();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TEntity Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public T ExecuteSql<T>(string query, params object[] parameters) where T : class
        {
            try
            {
                var rst = _db.Database.SqlQuery<T>(query, parameters).FirstOrDefault();
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteSql(string query)
        {
            try
            {
                _db.Database.SqlQuery<TEntity>(query);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
