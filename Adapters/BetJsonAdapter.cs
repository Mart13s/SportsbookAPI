using Newtonsoft.Json;
using SportsbookAPI.Models;

namespace SportsbookAPI.Adapters
{
    public class BetJsonAdapter : IBetAdapter
    {
        private readonly string _filePath = "./Data/Bets.json";

        public BetJsonAdapter(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<Bet> GetBets()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"File not found {_filePath}.");
            }

            var jsonData = File.ReadAllText(_filePath);
            if (jsonData == null) return new List<Bet>();

            IEnumerable<Bet> bets;

            try
            {
                bets = JsonConvert.DeserializeObject<IEnumerable<Bet>>(jsonData) ?? new List<Bet>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while reading bets: {ex.Message}.");
            }

            return bets;
        }

        public void AddBet(Bet bet)
        {
            var bets = GetBets().ToList();
            bets.Add(bet);

            try
            {
                var jsonData = JsonConvert.SerializeObject(bets);
                File.WriteAllText(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while writing to {_filePath}: {ex.Message}");
            }
        }
    }
}
