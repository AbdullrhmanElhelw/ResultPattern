namespace ResultPattern.Shared.Repositories.Abstractions;

public interface IGenericRepository<T> where T : class
{
    T? GetById(Guid id);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    IEnumerable<T> GetAll();

    IEnumerable<T> FindAll(Func<T, bool> key);
}