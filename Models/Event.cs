namespace SportsbookAPI.Models;

public class Event
{
    public int Id { get; set; }
    public bool IsLive { get; set; }
    public DateTime StartTime { get; set; }
    public IEnumerable<Odds>? Odds { get; set; }
}