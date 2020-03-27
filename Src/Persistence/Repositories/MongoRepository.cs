using System.Threading.Tasks;
using Humanizer;
using MongoDB.Driver;

namespace Persistence.Repositories
{
    public sealed class MongoRepository<T> : IMongoRepository<T>
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<T>(typeof(T).Name.Pluralize());
        }

        public Task InsertOneAsync(T value)
        {
            return _collection.InsertOneAsync(value);
        }
    }
}