using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IMongoRepository<T>
    {
        Task InsertOneAsync(T value);
    }
}