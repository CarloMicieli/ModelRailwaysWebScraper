using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ModelRailwayWorker.Contracts;

namespace ModelRailwayWorker
{
    public class Client
    {
        private readonly IRequestClient<HelloWorld> _client;
        private readonly ILogger<Client> _logger;

        public Client(IRequestClient<HelloWorld> client, ILogger<Client> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task Run()
        {
            var response = await _client.GetResponse<string>(new { Name = "Ciccins" });
            _logger.LogInformation(response.Message);
        }
    }
}