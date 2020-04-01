using Microsoft.Extensions.Logging;
using Persistence.Messages.Events;
using System.Threading.Tasks;
using Rebus.Handlers;

namespace Persistence.Handlers
{
    public class DatabaseEventsHandler : IHandleMessages<ResourceSaved>
    {
        private readonly ILogger<DatabaseEventsHandler> _logger;

        public DatabaseEventsHandler(ILogger<DatabaseEventsHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ResourceSaved message)
        {
            _logger.LogInformation($"Nothing to do, really : ResourceSaved = {message.Id}");
            return Task.CompletedTask;
        }
    }
}