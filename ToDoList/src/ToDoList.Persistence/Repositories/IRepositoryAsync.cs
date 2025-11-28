namespace ToDoList.Persistence.Repositories;

public interface IRepositoryAsync<T>
    where T : class
{
    public Task Create(T item);
    Task<IEnumerable<T>> ReadAll();
    Task<T?> ReadById(int id);
    public Task Update(T item);
    public Task Delete(T item);
}

