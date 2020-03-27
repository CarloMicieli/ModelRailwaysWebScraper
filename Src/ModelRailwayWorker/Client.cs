using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ModelRailwayWorker.Contracts;

namespace ModelRailwayWorker
{
    public class Client
    {
        private readonly ISendEndpointProvider _sendEndpoint;
        private readonly ILogger<Client> _logger;

        public Client(ISendEndpointProvider sendEndpoint, ILogger<Client> logger)
        {
            _sendEndpoint = sendEndpoint;
            _logger = logger;
        }

        public async Task Run()
        {
            await _sendEndpoint.Send<HelloWorld>(new { Name = "Ciccins " });
        }
    }
}