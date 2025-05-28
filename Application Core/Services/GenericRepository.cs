using ShopOwnerCore.Application_Core.Interface;
using ShopOwnerCore.Application_Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;


namespace ShopOwnerCore.Application_Core.Services
{
    public class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        private readonly ApplicationContext _context;
        private readonly IUnitWork _unitWork;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _unitWork = new UnitWork(context);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            if (!trackChanges)
            {
               return  _context.Set<T>().AsNoTracking();
            }
            else
            
               return  _context.Set<T>();
            
        }

        public T Find(Expression<Func<T,bool>> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression, bool trackChanges)
        {
            if (!trackChanges)
            {
                _context.Set<T>().Where(expression).AsNoTracking();
            }
            

             return _context.Set<T>().Where(expression);
        }

        public IQueryable<T> FindBy(Expression<Func<T,bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public T GetByUniqueId(string uniqueId)
        {
            return _context.Set<T>().Find(uniqueId);
        }

        public async Task<T> GetByUniqueIdAsync(string uniqueId)
        {
            return await _context.Set<T>().FindAsync(uniqueId);
        }


        
        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async  Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(int Id)
        {
            return _context.Set<T>().Find(Id);
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _unitWork.Save();
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().AddAsync(entity);
            await _unitWork.Commit();
            return entity;
        }

        public T Update(T Updated)
        {
            if(Updated == null)
            {
                return null;
            }

            _context.Set<T>().Attach(Updated);
            _context.Entry(Updated).State = EntityState.Modified;
            _unitWork.Save();
            return Updated;
        }

        public async Task<T> UpdateAsync(T Updated)
        {
            if(Updated == null)
            {
                return null;
            }
            _context.Set<T>().Attach(Updated);
            _context.Entry(Updated).State = EntityState.Modified;
            await  _unitWork.Commit();
            return Updated;

        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _unitWork.Save();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _unitWork.Commit();
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

       public  IEnumerable<T> Filter(Expression<Func<T, bool>> filter= null, Func<IQueryable<T>,IOrderedQueryable<T>> orderby = null , string includeProperties = "", int? page= null, int ?pageSize=null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(orderby != null)
            {
                query = orderby(query);

            }
            if(includeProperties != null)
            {
               foreach(var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperties);
                }
            }
            if(page!= null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

                
            }
            return query.ToList();

        }

        public bool Exist(Expression<Func<T,bool>> predicate)
        {
            var exist = _context.Set<T>().Where(predicate);
            return exist.Any() ? true : false;
        }

        

      
     

      
    }
}
