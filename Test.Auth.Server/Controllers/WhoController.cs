using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Test.Auth.Server.Controllers
{
    [ApiController]
    [Route("api/who")]
    public class WhoController : ControllerBase
    {
        private readonly ILogger<TimeController> _logger;

        public WhoController(ILogger<TimeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<GetTime> GetTime()
        {
            var user = this.User;
            object who;

            if (user == null)
            {
                who = new
                {
                    Unknown = true,
                };
            }
            else
            {
                who = new
                {
                    user.Identity.IsAuthenticated,
                    user.Identity.AuthenticationType,
                    user.Identity.Name
                };
            }

            return Ok(who);
        }
    }
}
