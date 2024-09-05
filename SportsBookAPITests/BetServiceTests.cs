using NSubstitute;
using SportsbookAPI.Adapters;
using SportsbookAPI.Models;
using SportsbookAPI.Services;

namespace SportsbookAPITests
{
    public class BetServiceTests
    {
        private readonly IPlayerAdapter _playerAdapter;
        private readonly IEventAdapter _eventAdapter;
        private readonly IBetAdapter _betAdapter;
        private readonly BetService _betService;

        public BetServiceTests()
        {
            _playerAdapter = Substitute.For<IPlayerAdapter>();
            _eventAdapter = Substitute.For<IEventAdapter>();
            _betAdapter = Substitute.For<IBetAdapter>();
            _betService = new BetService(_playerAdapter, _eventAdapter, _betAdapter);
        }

        [Fact]
        public void PlaceBet_ShouldThrowArgumentNullException_WhenBetPlacementIsNull()
        {
            BetPlacement betPlacement = null;

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Bet placement is required.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowArgumentNullException_WhenPlayerIdIsNull()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = null,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("PlayerId is required.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowArgumentException_WhenWagerOddsIsZeroOrNegative()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 0,
                Wager = 100M
            };

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("WagerOdds must be a number above zero.", exception.Message);

            betPlacement.WagerOdds = -1.5M;
            exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("WagerOdds must be a number above zero.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenPlayerNotFound()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            _playerAdapter.GetPlayers().Returns(new List<Player>());

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Player with Id 1 was not found.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenPlayerBalanceInsufficient()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var player = new Player { Id = 1, Balance = 50M };
            _playerAdapter.GetPlayers().Returns(new List<Player> { player });

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Player has insufficient balance to place bet.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenEventNotFound()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            _playerAdapter.GetPlayers().Returns(new List<Player> { new Player { Id = 1, Balance = 100M } });
            _eventAdapter.GetEvents().Returns(new List<Event>());

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Event with Id 1 was not found.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenEventHasNoOdds()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var bettingEvent = new Event { Id = 1, Odds = new List<Odds>() };
            _playerAdapter.GetPlayers().Returns(new List<Player> { new Player { Id = 1, Balance = 100M } });
            _eventAdapter.GetEvents().Returns(new List<Event> { bettingEvent });

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Event with Id 1 has no odds.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenEventIsClosed()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var bettingEvent = new Event { Id = 1, Odds = new List<Odds> { new Odds { Id = 1, Value = 1.5M } }, IsLive = false, StartTime = DateTime.Now.AddMinutes(-10) };
            _playerAdapter.GetPlayers().Returns(new List<Player> { new Player { Id = 1, Balance = 100M } });
            _eventAdapter.GetEvents().Returns(new List<Event> { bettingEvent });

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Event with Id 1. Betting is closed.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenEventNotLiveYet()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var bettingEvent = new Event { Id = 1, Odds = new List<Odds> { new Odds { Id = 1, Value = 1.5M } }, IsLive = true, StartTime = DateTime.Now.AddMinutes(10) };
            _playerAdapter.GetPlayers().Returns(new List<Player> { new Player { Id = 1, Balance = 100M } });
            _eventAdapter.GetEvents().Returns(new List<Event> { bettingEvent });

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Event with Id 1. Betting is not open yet.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenOddsNotFound()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var bettingEvent = new Event { Id = 1, IsLive = false, StartTime = DateTime.Now.AddDays(1), Odds = new List<Odds>() { new Odds() { Id = 333, Value = 1 } } };
            _playerAdapter.GetPlayers().Returns(new List<Player> { new Player { Id = 1, Balance = 100M } });
            _eventAdapter.GetEvents().Returns(new List<Event> { bettingEvent });

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Odds with Id 1 not found.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenOddsHaveNegativeValue()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var odds = new Odds { Id = 1, Value = -1.5M };
            var bettingEvent = new Event { Id = 1, IsLive = false, StartTime = DateTime.Now.AddDays(1), Odds = new List<Odds> { odds } };
            _playerAdapter.GetPlayers().Returns(new List<Player> { new Player { Id = 1, Balance = 100M } });
            _eventAdapter.GetEvents().Returns(new List<Event> { bettingEvent });

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Odds with Id 1 must have positive odds.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldThrowException_WhenOddsDifferenceIsTooLarge()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 2.0M,
                Wager = 100M
            };

            var odds = new Odds { Id = 1, Value = 1.5M };
            var bettingEvent = new Event { Id = 1, Odds = new List<Odds> { odds }, IsLive = true, StartTime = DateTime.Now.AddMinutes(-10) };
            _playerAdapter.GetPlayers().Returns(new List<Player> { new Player { Id = 1, Balance = 100M } });
            _eventAdapter.GetEvents().Returns(new List<Event> { bettingEvent });

            var exception = Assert.Throws<Exception>(() => _betService.PlaceBet(betPlacement));
            Assert.Equal("Bet rejected.", exception.Message);
        }

        [Fact]
        public void PlaceBet_ShouldReturnBetId_WhenBetIsSuccessful()
        {
            var betPlacement = new BetPlacement
            {
                PlayerId = 1,
                OddsId = 1,
                EventId = 1,
                WagerOdds = 1.5M,
                Wager = 100M
            };

            var odds = new Odds { Id = 1, Value = 1.5M };
            var bettingEvent = new Event { Id = 1, Odds = new List<Odds> { odds }, IsLive = true, StartTime = DateTime.Now.AddMinutes(-10) };
            var player = new Player { Id = 1, Balance = 100M };

            _playerAdapter.GetPlayers().Returns(new List<Player> { player });
            _eventAdapter.GetEvents().Returns(new List<Event> { bettingEvent });
            _betAdapter.GetBets().Returns(new List<Bet>());
            _betAdapter.When(b => b.AddBet(Arg.Any<Bet>())).Do(ci => { });

            var betId = _betService.PlaceBet(betPlacement);

            Assert.NotNull(betId);
        }
    }
}
