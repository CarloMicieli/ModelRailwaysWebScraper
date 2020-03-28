using System;
using System.Threading;
using System.Threading.Tasks;
using App.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace App
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IBus _bus;

        public WorkerService(IBus bus, ILogger<WorkerService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting...");

            await _bus.Send(new Job 
            { 
                Name = "Hello"
            });
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping...");
            return Task.CompletedTask;
        }
    }
}
