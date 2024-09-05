namespace SportsbookAPI.Models;

public class Bet
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int OddId { get; set; }
    public decimal WagerOdds { get; set; }
    public decimal Wager { get; set; }
}
