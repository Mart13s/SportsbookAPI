using SportsbookAPI.Models;

namespace SportsbookAPI.Services
{
    public interface IBetLogger
    {
        IEnumerable<BetLogEntry> GetBetLogEntries();
        void Log(int? eventId, int? oddsId, int? resultCode, string message, int? betId = null);
    }
}