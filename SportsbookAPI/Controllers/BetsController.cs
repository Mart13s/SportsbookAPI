using Microsoft.AspNetCore.Mvc;
using SportsbookAPI.Models;
using SportsbookAPI.Services;

namespace SportsbookAPI.Controllers;

[ApiController]
[Route("api/bets")]
public class BetsController : ControllerBase
{
    private readonly IBetService _betService;
    private readonly IBetLogger _logger;

    public BetsController(IBetService betService, IBetLogger betLogger)
    {
        _betService = betService;
        _logger = betLogger;
    }

    [HttpGet]
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

    [HttpPost]
    public IActionResult PlaceBet([FromBody] BetPlacement bet)
    {
        int? betId;
        try
        {
            betId = _betService.PlaceBet(bet);
        }
        catch (Exception ex)
        {
            _logger.Log(bet.EventId, bet.OddsId, 400, ex.Message);
            return BadRequest("Failed to place bet: " + ex.Message);
        }

        _logger.Log(bet.EventId, bet.OddsId, 200, $"Bet with id {betId} created successfully.", betId);
        return Ok($"Bet with id {betId} created successfully.");
    }

    [HttpGet("log")]
    public IActionResult GetAllBetsLog()
    {
        List<BetLogEntry> betLogEntries;

        try
        {
            betLogEntries = _logger.GetBetLogEntries().ToList();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(betLogEntries);
    }
}
