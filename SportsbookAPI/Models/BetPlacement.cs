using System.ComponentModel.DataAnnotations;

namespace SportsbookAPI.Models
{
    public class BetPlacement
    {
        public int? PlayerId { get; set; }
        public int? EventId { get; set; }
        public int? OddsId { get; set; }
        public decimal? WagerOdds { get; set; }
        public decimal? Wager { get; set; }
    }
}
