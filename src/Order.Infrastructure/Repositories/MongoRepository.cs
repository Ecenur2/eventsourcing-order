using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Order.Domain.Repositories;

namespace Order.Infrastructure.Repositories;

public class MongoRepository : IMongoRepository
{
    private readonly IMongoDatabase _database;

    public MongoRepository(IMongoDatabase database)
    {
        _database = database;
    }

    private IMongoCollection<T> Collection<T>(string collectionName)
        => _database.GetCollection<T>(collectionName);

    public async Task<List<T>> SearchAsync<T>(Expression<Func<T, bool>> expression, int limit) where T : class, new()
        => await Collection<T>(typeof(T).Name).Find(expression).Limit(limit).ToListAsync();
    
    public async Task<T> GetAsync<T>(Expression<Func<T, bool>> expression)
        where T : class, new()
        => await Collection<T>(typeof(T).Name).Find(expression).FirstOrDefaultAsync();
    
    public async Task<T> GetLastValueAsync<T>(Expression<Func<T, bool>> expression)
        where T : class, new()
        => await Collection<T>(typeof(T).Name).Find(expression).Sort(new BsonDocument("$natural", -1)).FirstOrDefaultAsync();

    public async Task InsertOneAsync<T>(T item) where T : class, new()
        => await Collection<T>(typeof(T).Name).InsertOneAsync(item);

    public async Task ReplaceOneAsync<T>(Expression<Func<T, bool>> expression, T item)
        where T : class, new()
        => await Collection<T>(typeof(T).Name).ReplaceOneAsync(expression, item);
}