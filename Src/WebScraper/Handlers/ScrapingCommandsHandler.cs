using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Rebus.Handlers;
using Rebus.Bus;
using WebScraper.Messages.Commands;
using Persistence.Messages.Events;
using System;

namespace WebScraper.Handlers
{
    public class ScrapingCommandsHandler : IHandleMessages<ScrapeWebSiteCommand>
    {
        private readonly ILogger<ScrapingCommandsHandler> _logger;
        private readonly IBus _bus;

        public ScrapingCommandsHandler(IBus bus, ILogger<ScrapingCommandsHandler> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task Handle(ScrapeWebSiteCommand command)
        {
            _logger.LogInformation($"Handling ScrapeWebSiteCommand = {command.WebsiteUrl}");
            await _bus.Publish(new ResourceSaved
            {
                Id = Guid.NewGuid().ToString()
            });
        }
    }
}