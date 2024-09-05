using Microsoft.AspNetCore.Mvc;
using SportsbookAPI.Adapters;
using SportsbookAPI.Models;
using SportsbookAPI.Services;

namespace SportsbookAPI.Controllers;

[ApiController]
[Route("api/bets")]
public class BetsController : ControllerBase
{
    private readonly IBetService _betService;

    public BetsController(IBetService betService)
    {
        _betService = betService;
    }

    [HttpGet(Name = "GetAllBets")]
    public IActionResult Get()
    {
        List<Bet> bets;

        try
        {
            bets = _betService.GetBets().ToList();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(bets);
    }

    [HttpPost(Name = "Place Bet")]
    public IActionResult PlaceBet([FromBody] BetPlacement bet)
    {
        if (bet == null) return BadRequest("Bet is null.");
        if (bet.PlayerId == null) return BadRequest("PlayerId is required.");
        if (bet.OddsId == null) return BadRequest("OddsId is required.");
        if (bet.EventId == null) return BadRequest("EventId is required.");
        if (bet.WagerOdds == null) return BadRequest("WagerOdds is required.");
        if (bet.Wager == null) return BadRequest("Wager is required.");

        int? betId = null;
        try
        {
            betId = _betService.PlaceBet(bet);
        }
        catch (Exception ex)
        {
            BadRequest("Failed to place bet: " + ex.Message);
        }

        return Ok($"Bet with id {betId} created successfully.");
    }
}
