namespace ShopOwnerCore.Application_Core.Interface
{
    public interface IUnitWork
    {
        //Hide Business Logic
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> Commit();

        void RollBack();

        void Save();

        void Dispose();




    }
}
