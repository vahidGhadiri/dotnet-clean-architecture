namespace API.Application.Common.Ports;

public interface IRepository<T> where T : class
{
    Task Add(T entity);
    Task Update(T entity);
    Task Remove(T entity);
}
