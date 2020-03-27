using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelRailwayWorker.Consumers;
using ModelRailwayWorker.Contracts;
using ModelRailwayWorker.Configuration;
using Serilog;
using MongoDB;
using WebScraper;

namespace ModelRailwayWorker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            services.AddMongoDB(cfg =>
            {
                cfg.UseConnectionString(Configuration["MongoUri"]);
                cfg.UseDatabaseName("ModelRailwaysDb");
            });

            services.AddWebScrapers(b =>
            {
                b.ModellbahnshopLippe = true;
            });

            var rabbitMq = new RabbitMqConfig();
            Configuration.GetSection("RabbitMq").Bind(rabbitMq);

            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                return Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    sbc.Host(rabbitMq.Host, rmq =>
                    {
                        rmq.Username(rabbitMq.Username);
                        rmq.Password(rabbitMq.Password);
                    });
                    sbc.PurgeOnStartup = true;

                    EndpointConvention.Map<HelloWorld>(new Uri("queue:hello_queue"));

                    sbc.ReceiveEndpoint("hello_queue", ep =>
                    {
                        ep.Consumer<HelloWorldConsumer>();
                    });
                });
            }

            services.AddMassTransit(cfg =>
            {
                cfg.AddBus(CreateBus);
                cfg.AddConsumersFromNamespaceContaining(typeof(HelloWorldConsumer));
            });

            services.AddScoped<Client>();
        }
    }
}