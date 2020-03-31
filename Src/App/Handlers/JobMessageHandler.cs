using Microsoft.Extensions.Logging;
using Persistence.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;
using WebScraper.Messages;

namespace App.Handlers
{
    public class JobMessageHandler 
        : IHandleMessages<ScrapeWebSiteCommand>
        , IHandleMessages<ResourceSaved>
    {
        private readonly ILogger<JobMessageHandler> _logger;
        private readonly IBus _bus;

        public JobMessageHandler(IBus bus, ILogger<JobMessageHandler> logger)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(ScrapeWebSiteCommand message)
        {
            _logger.LogInformation($"Nothing to do, really : ScrapeWebSiteCommand = {message.WebsiteUrl}");

            await _bus.Publish(new ResourceSaved
            {
                Id = Guid.NewGuid().ToString()
            });
        }

        public Task Handle(ResourceSaved message)
        {
            _logger.LogInformation($"Nothing to do, really : ResourceSaved = {message.Id}");
            return Task.CompletedTask;
        }
    }
}
