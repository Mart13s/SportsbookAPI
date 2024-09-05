using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsbookAPI.Models;

namespace SportsbookAPI.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class BetsController : ControllerBase
    {
        private readonly ILogger<BetsController> _logger;

        public BetsController(ILogger<BetsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAllBetsLog")]
        public IActionResult Get()
        {
            return Ok(new List<BetDto>());
        }
}
