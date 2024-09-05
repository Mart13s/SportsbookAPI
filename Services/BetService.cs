using SportsbookAPI.Adapters;
using SportsbookAPI.Models;

namespace SportsbookAPI.Services
{
    public class BetService : IBetService
    {
        private readonly IPlayerAdapter _playerAdapter;
        private readonly IEventAdapter _eventAdapter;
        private readonly IBetAdapter _betAdapter;

        public BetService(IPlayerAdapter playerAdapter, IEventAdapter eventAdapter, IBetAdapter betAdapter)
        {
            _playerAdapter = playerAdapter;
            _eventAdapter = eventAdapter;
            _betAdapter = betAdapter;
        }

        public IEnumerable<Bet> GetBets()
        {
            return _betAdapter.GetBets();
        }

        public int? PlaceBet(BetPlacement betPlacement)
        {
            ValidateBet(betPlacement);
            ValidatePlayer(betPlacement);
            var bettingEvent = ValidateEvent(betPlacement);
            var odds = ValidateOdds(betPlacement, bettingEvent);

            if (CheckOddsDifference(betPlacement, bettingEvent, odds))
            {
                Bet bet = new Bet()
                {
                    Id = new Random().Next(),
                    EventId = (int)betPlacement.EventId,
                    PlayerId = (int)betPlacement.PlayerId,
                    OddsId = (int)betPlacement.OddsId,
                    WagerOdds = (decimal)betPlacement.WagerOdds,
                    Wager = (decimal)betPlacement.Wager
                };

                try
                {
                    _betAdapter.AddBet(bet);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error adding bet: {bet.Id}. {ex.Message}.");
                }
            }

            throw new Exception("Bet rejected.");
        }

        private void ValidateBet (BetPlacement betPlacement)
        {
            if (betPlacement == null) throw new ArgumentNullException("Bet placement is required.");
            if (betPlacement.PlayerId == null) throw new ArgumentNullException("PlayerId is required.");
            if (betPlacement.OddsId == null) throw new ArgumentNullException("OddsId is required.");
            if (betPlacement.EventId == null) throw new ArgumentNullException("EventId is required.");
            if (betPlacement.WagerOdds == null) throw new ArgumentNullException("WagerOdds is required.");
            if (betPlacement.Wager == null) throw new ArgumentNullException("Wager is required.");
            if (betPlacement.WagerOdds <= 0) throw new ArgumentException("WagerOdds must be a number above zero.");
            if (betPlacement.Wager <= 0) throw new ArgumentException("Wager must be a number above zero.");
        }

        private Player ValidatePlayer(BetPlacement betPlacement)
        {
            var player = _playerAdapter.GetPlayers().FirstOrDefault(p => p.Id == betPlacement.PlayerId);

            if (player == null) throw new Exception($"Player with Id {betPlacement.PlayerId} was not found.");
            if (player.Balance <= 0) throw new Exception("Player has insufficient balance to place bet.");
            if (player.Balance < betPlacement.Wager) throw new Exception("Player has insufficient balance to place bet.");

            return player;
        }

        private Event ValidateEvent(BetPlacement betPlacement)
        {
            var bettingEvent = _eventAdapter.GetEvents().FirstOrDefault(e => e.Id == betPlacement.EventId);

            if (bettingEvent == null) throw new Exception($"Event with Id {betPlacement.EventId} was not found.");
            if (bettingEvent.Odds == null || !bettingEvent.Odds.Any()) throw new Exception($"Event with Id {betPlacement.EventId} has no odds.");
            if (!bettingEvent.IsLive && bettingEvent.StartTime < DateTime.Now) throw new Exception($"Event with Id {betPlacement.EventId}. Betting is closed.");
            if (bettingEvent.IsLive && bettingEvent.StartTime > DateTime.Now) throw new Exception($"Event with Id {betPlacement.EventId}. Betting is not open yet.");

            return bettingEvent;
        }

        private Odds ValidateOdds(BetPlacement betPlacement, Event bettingEvent)
        {
            var odds = bettingEvent?.Odds?.FirstOrDefault(o => o.Id == betPlacement.OddsId);

            if (odds == null) throw new Exception($"Odds with Id {betPlacement.OddsId} not found.");
            if (odds.Value <= 0) throw new Exception($"Odds with Id {betPlacement.OddsId} must have positive odds.");

            return odds;
        }

        private bool CheckOddsDifference(BetPlacement betPlacement, Event bettingEvent, Odds odds)
        {
            if (betPlacement == null || betPlacement.WagerOdds == null || odds == null) return false;

            decimal diff = Math.Abs((decimal)(odds.Value - betPlacement.WagerOdds));
            if ((bettingEvent.IsLive && diff < odds.Value * 0.1M)
                || (!bettingEvent.IsLive && diff > odds.Value * 0.05M))
            {
                return true;
            }

            return false;
        }
    }
}
