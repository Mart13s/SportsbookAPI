using SportsbookAPI.Models;

namespace SportsbookAPI.Adapters
{
    public interface IPlayerAdapter
    {
        IEnumerable<Player> GetPlayers();
    }
}