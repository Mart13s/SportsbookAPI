using Newtonsoft.Json;
using SportsbookAPI.Models;

namespace SportsbookAPI.Services
{
    public class BetLogger : IBetLogger
    {
        private readonly string _filePath = "./Data/BetsLog.json";

        public BetLogger(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<BetLogEntry> GetBetLogEntries()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"File not found {_filePath}.");
            }

            var jsonData = File.ReadAllText(_filePath);
            if (jsonData == null) return new List<BetLogEntry>();

            IEnumerable<BetLogEntry> loggedBets;

            try
            {
                loggedBets = JsonConvert.DeserializeObject<IEnumerable<BetLogEntry>>(jsonData) ?? new List<BetLogEntry>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while reading bet log: {ex.Message}.");
            }

            return loggedBets;
        }

        public void Log(int? eventId, int? oddsId, int? resultCode, string message, int? betId = null)
        {
            var loggedBets = GetBetLogEntries().ToList();

            BetLogEntry betLogEntry = new()
            {
                EventId = eventId,
                OddsId = oddsId,
                ResultCode = resultCode,
                Message = message,
                BetId = betId
            };

            loggedBets.Add(betLogEntry);

            try
            {
                var jsonData = JsonConvert.SerializeObject(loggedBets);
                File.WriteAllText(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while writing to {_filePath}: {ex.Message}");
            }
        }
    }
}
