using Microsoft.AspNetCore.Mvc;
using SportsbookAPI.Adapters;
using SportsbookAPI.Models;

namespace SportsbookAPI.Controllers;

[ApiController]
[Route("api/bets")]
public class BetsController : ControllerBase
{
    IBetAdapter _betAdapter { get; set; }

    public BetsController(IBetAdapter betAdapter)
    {
        _betAdapter = betAdapter;
    }

    [HttpGet(Name = "GetAllBets")]
    public IActionResult Get()
    {
        List<Bet> bets;

        try
        {
            bets = _betAdapter.GetBets().ToList();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(bets);
    }
}
