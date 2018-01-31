using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Common.Entities.Interfaces;
using Framework.Common.Entities.Pagination;

namespace Framework.Data.Repository
{
    public interface IRepository<T, TKey> : IDisposable 
        where T : IEntity<TKey> 
    {
        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetAllAsync();

        IQueryable<T> GetAll(Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);

        PagedResultList<T> GetAll(PagingCriteria paging, params Expression<Func<T, object>>[] includes);
        Task<PagedResultList<T>> GetAllAsync(PagingCriteria paging, params Expression<Func<T, object>>[] includes);

        PagedResultList<T> GetAll(PagingCriteria paging, Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);
        Task<PagedResultList<T>> GetAllAsync(PagingCriteria paging, Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);

        T Get(TKey id, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(TKey id, params Expression<Func<T, object>>[] includes);

        T Get(Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);

        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
        void Delete(TKey id);
        //bool Commit();
    }
    
}
