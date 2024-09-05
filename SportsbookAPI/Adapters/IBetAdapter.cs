using SportsbookAPI.Models;

namespace SportsbookAPI.Adapters
{
    public interface IBetAdapter
    {
        void AddBet(Bet bet);
        IEnumerable<Bet> GetBets();
    }
}