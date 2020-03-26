using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelRailwayWorker.Consumers;
using ModelRailwayWorker.Contracts;
using ModelRailwayWorker.Configuration;
using Serilog;

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

            var rabbitMq = new RabbitMqConfig();
            Configuration.GetSection("RabbitMq").Bind(rabbitMq);

            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                return Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(rabbitMq.Host, rmq =>
                    {
                        rmq.Username(rabbitMq.Username);
                        rmq.Password(rabbitMq.Password);
                    });
                    cfg.PurgeOnStartup = true;

                    cfg.ReceiveEndpoint("hello_queue", ep => 
                    {
                        ep.Consumer<HelloWorldConsumer>();
                    });
                });
            }

            services.AddMassTransit(cfg =>
            {
                cfg.AddBus(CreateBus);
                cfg.AddConsumersFromNamespaceContaining(typeof(HelloWorldConsumer));
                cfg.AddRequestClient<HelloWorld>();
            });

            services.AddScoped<Client>();
        }
    }
}