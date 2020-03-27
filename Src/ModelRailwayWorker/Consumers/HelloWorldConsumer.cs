using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ModelRailwayWorker.Contracts;

namespace ModelRailwayWorker.Consumers
{
    public class HelloWorldConsumer : IConsumer<HelloWorld>
    {
        // private readonly ILogger<HelloWorldConsumer> _logger;

        /*public HelloWorldConsumer(ILogger<HelloWorldConsumer> logger)
        {
            _logger = logger;
        }*/

        public async Task Consume(ConsumeContext<HelloWorld> context)
        {
            // _logger.LogInformation($"Hello, {context.Message.Name}");
            await context.RespondAsync<Greeting>(new { Greeting = $"Hello, {context.Message.Name}" });
        }
    }
}