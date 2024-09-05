using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsbookAPI.Adapters;
using SportsbookAPI.Models;

namespace SportsbookAPI.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerAdapter _playerAdapter;

        public PlayersController(IPlayerAdapter playerAdapter)
        {
            _playerAdapter = playerAdapter;
        }

        [HttpGet(Name = "GetAllPlayers")]
        public IActionResult Get()
        {
            List<Player> players;

            try
            {
                players = _playerAdapter.GetPlayers().ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(players);
        }
    }
}
