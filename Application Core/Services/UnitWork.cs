using ShopOwnerCore.Application_Core.Data;
using ShopOwnerCore.Application_Core.Interface;


namespace ShopOwnerCore.Application_Core.Services
{
    public class UnitWork:IUnitWork,IDisposable
    {
        private readonly ApplicationContext applicationContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public Dictionary<Type, object> Repositories
        {
            get { return _repositories; }
            set { Repositories = value; }
        }

        public UnitWork(ApplicationContext context)
        {
            applicationContext = context;
        }

       public  IGenericRepository<T> Repository<T>() where T:class
        {
            if (Repositories.ContainsKey(typeof(T)))
            {
                return Repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(applicationContext);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public async Task<int> Commit()
        {
            return await applicationContext.SaveChangesAsync();
        }

        public void RollBack()
        {
            applicationContext.ChangeTracker.Entries().ToList().ForEach(e => e.Reload());
        }
        public void Save()
        {
            applicationContext.SaveChanges();
        }

        public void Dispose()
        {
            applicationContext.Dispose();
        }
    }
}
