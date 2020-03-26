using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelRailwayWorker.Contracts;

namespace ModelRailwayWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _bus;
        private readonly IServiceScopeFactory _services;

        public Worker(IServiceScopeFactory services, IBusControl bus, ILogger<Worker> logger)
        {
            _logger = logger;
            _bus = bus;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting bus");
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);

            for (int i = 0; i < 50; i++)
            {
                await Task.Delay(5000);
                await _bus.Publish<HelloWorld>(new { Name = "Ciccins" });
            }



            // To overcome lifetime differences between the Worker (singleton)
            // and mass-transing request clients (scoped)
            //using (var scope = _services.CreateScope())
            //{
            //    var client = scope.ServiceProvider.GetRequiredService<Client>();
            //    await client.Run();
            //}
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping bus");
            return _bus.StopAsync(cancellationToken);
        }
    }
}
