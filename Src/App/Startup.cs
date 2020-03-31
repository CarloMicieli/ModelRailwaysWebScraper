using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using MongoDB;
using WebScraper;
using App.Configuration;
using Rebus.ServiceProvider;
using Rebus.Config;
using Rebus.Transport.InMem;
using Rebus.Routing.TypeBased;
using Persistence.Messages;
using WebScraper.Messages;
using Rebus.Persistence.InMem;

namespace App
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

            // Register handlers 
            services.AutoRegisterHandlersFromAssemblyOf<Startup>();

            // Configure and register Rebus
            services.AddRebus(configure => configure
                .Options(o => 
                {
                    o.SetNumberOfWorkers(4);
                    o.SetMaxParallelism(16);
                })
                .Logging(l => l.Serilog())
                .Transport(t => 
                {
                    t.UseInMemoryTransport(new InMemNetwork(), "scraper-messages");
                })
                .Subscriptions(s => s.StoreInMemory())
                .Routing(r => 
                    r.TypeBased()
                        .MapAssemblyOf<ResourceSaved>("persistence-messages")
                        .MapAssemblyOf<ScrapeWebSiteCommand>("scraper-messages")));
                
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplicationServices.UseRebus();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
