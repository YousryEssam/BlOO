namespace BlOO.Repositories
{
    public interface IGenericRepository<T> 
    {
        void DeleteById(int id);
        void Delete(T entity);
        void Insert(T entity);
        T GetById(int id);
        void Save();
        void Update(T entity);
    }
}
