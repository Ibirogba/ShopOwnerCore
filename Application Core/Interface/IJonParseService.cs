namespace ShopOwnerCore.Application_Core.Interface
{
    public interface IJonParseService<T> where T:class
    {
        T ToObject(string json);
    }
}
