using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using MongoDB;
using App.Configuration;
using Rebus.ServiceProvider;
using Rebus.Config;
using Rebus.Transport.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Persistence.InMem;
using Rebus.Retry.Simple;
using Rebus.Bus;
using Core;
using System.Linq;
using Persistence.Handlers;
using Persistence.Messages.Events;
using WebScraper;
using WebScraper.Handlers;
using WebScraper.Messages.Commands;

namespace App
{
    public class Startup
    {
        private const string QueueName = "model-railways";

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

            services.AddControllers();

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

            services.AutoRegisterHandlersFromAssemblyOf<ScrapingCommandsHandler>();
            services.AutoRegisterHandlersFromAssemblyOf<DatabaseEventsHandler>();

            services.AddRebus(configure => configure
                .Options(o =>
                {
                    o.SetNumberOfWorkers(4);
                    o.SetMaxParallelism(16);
                    o.SimpleRetryStrategy(maxDeliveryAttempts: 3);
                    o.LogPipeline(verbose: true);
                })
                .Logging(l => l.Serilog())
                .Transport(t =>
                {
                    t.UseInMemoryTransport(new InMemNetwork(), QueueName);
                })
                .Subscriptions(s => s.StoreInMemory())
                .Routing(r =>
                    r.TypeBased()
                        .MapAssemblyOf<ResourceSaved>(QueueName)
                        .MapAssemblyOf<ScrapeWebSiteCommand>(QueueName)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplicationServices.UseRebus(bus =>
            {
                SubscribeEvents(bus);
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SubscribeEvents(IBus bus)
        {
            var markerInterface = typeof(IEvent);
            var eventTypes = typeof(ResourceSaved).Assembly
                .GetTypes()
                .Where(it => markerInterface.IsAssignableFrom(it));

            foreach (var eventType in eventTypes)
            {
                // Bad practice
                bus.Subscribe(eventType).GetAwaiter().GetResult();
            }
        }
    }
}
