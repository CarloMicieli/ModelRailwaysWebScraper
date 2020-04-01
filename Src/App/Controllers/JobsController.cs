using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rebus.Bus;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IBus bus, ILogger<JobsController> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
