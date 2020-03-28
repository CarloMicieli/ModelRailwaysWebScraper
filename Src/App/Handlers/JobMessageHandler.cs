using App.Messages;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System.Threading.Tasks;

namespace App.Handlers
{
    public class JobMessageHandler : IHandleMessages<Job>
    {
        private readonly ILogger<JobMessageHandler> _logger;

        public JobMessageHandler(ILogger<JobMessageHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(Job message)
        {
            _logger.LogInformation("Nothing to do, really");
            return Task.CompletedTask;
        }
    }
}
