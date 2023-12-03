namespace Work.Interfaces
{
    public interface IRepository<T,K>
    {
        void Create(T obj);
        T Read(K key);    
        IEnumerable<T> ReadAll();        
        void Update(T obj);
        void Remove(T obj);
    }
}
