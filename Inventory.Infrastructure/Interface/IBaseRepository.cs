
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> GetCount(Expression<Func<T, bool>> filter = null, string includeProperties = "");
        T GetById(int id);
        Task<T> GetByIdAsync(int id);

         IEnumerable<T> GetAllIEnumerable(Expression<Func<T, bool>> filter = null, string includeProperties = "");
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = "");
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "");
        //Task<T> Update(T entity);

        Task<bool> Update(T entity);
        Task<bool> Update(List<T> entityList);
        Task<IEnumerable<T>> GetAllAsync();

        public Task<List<T>> GetAll();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "");
        IQueryable<T> GetWhere(Expression<Func<T, bool>> filter = null, string includeProperties = "");
        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = SortingOrder.Ascending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = SortingOrder.Ascending);
        T Add(T entity);
        Task<T> AddAsync(T entity);

        Task<bool> GetAny(Expression<Func<T, bool>> filter = null, string includeProperties = "");
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T UpdateX(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        Task<bool> DeleteList(List<T> entityList);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
    
    }
}
