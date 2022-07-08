using System.Linq.Expressions;

namespace Order.Domain.Repositories;

public interface IMongoRepository
{
    Task<List<T>> SearchAsync<T>(Expression<Func<T, bool>> expression, int limit) where T : class, new();
    Task<T> GetAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();

    Task<T> GetLastValueAsync<T>(Expression<Func<T, bool>> expression) where T : class, new();

    Task InsertOneAsync<T>(T item) where T : class, new();
    
    Task ReplaceOneAsync<T>(Expression<Func<T, bool>> expression, T item) where T : class, new();
}