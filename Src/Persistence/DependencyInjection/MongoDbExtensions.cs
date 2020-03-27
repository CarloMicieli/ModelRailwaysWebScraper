using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Persistence.Repositories;

namespace MongoDB
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection UseMongoDB(this IServiceCollection services, Action<MongoDbBuilder> buildAction)
        {
            var b = new MongoDbBuilder();
            buildAction(b);

            var client = new MongoClient(b.ConnectionString);

            services.AddSingleton<MongoClient>(client);
            services.AddSingleton<IMongoDatabase>(client.GetDatabase(b.DatabaseName));
            services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            return services;
        }
    }

    public class MongoDbBuilder
    {
        public string ConnectionString { private set; get; }
        public string DatabaseName { private set; get; }

        public void UseConnectionString(string connectionString) => ConnectionString = connectionString;
        public void UseDatabaseName(string dbName) => DatabaseName = dbName;
    }
}