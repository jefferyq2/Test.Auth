using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Test.Auth.Server.Controllers
{
    [ApiController]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        private readonly ILogger<TimeController> _logger;

        public TimeController(ILogger<TimeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<GetTime> GetTime()
        {
            return Ok(new GetTime());
        }
    }

    public class GetTime
    {
        public DateTime Now { get; set; } = DateTime.Now;
        public DateTime UtcNow { get; set; } = DateTime.UtcNow;
    }
}
