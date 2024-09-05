namespace SportsbookAPI.Models
{
    public class BetLogEntry
    {
        public int? BetId { get; set; }
        public int? EventId { get; set; }
        public int? OddsId { get; set; }
        public int? ResultCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
