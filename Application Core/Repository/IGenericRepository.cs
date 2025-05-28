using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ShopOwnerCore.Application_Core.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> FindAll(bool trackChanges);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        T Find(Expression<Func<T, bool>> expression);

        IQueryable<T> Query();

        ICollection<T> GetAll();

        Task<ICollection<T>> GetAllAsync();

        T GetByUniqueId(string uniqueId);

        Task<T> GetByUniqueIdAsync(string uniqueId);

        T GetById(int Id);

        Task<T> GetByIdAsync(int Id);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        T Update(T updated);

        Task<T> UpdateAsync(T entity);

       
        void Delete(T entity);

        Task<int> DeleteAsync(T entity);

        int Count();

        Task<int> CountAsync();

      
        

       

        IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? page=null, int? paseSize=null );

        bool Exist(Expression<Func<T, bool>> predicate);

        






    }
}
