using SportsbookAPI.Models;

namespace SportsbookAPI.Services
{
    public interface IBetService
    {
        IEnumerable<Bet> GetBets();
        int? PlaceBet(BetPlacement betPlacement);
    }
}