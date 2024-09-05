using System.ComponentModel.DataAnnotations;

namespace SportsbookAPI.Models;

public class Odds
{
    [Required(ErrorMessage = "Odds Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Odds are required.")]
    public decimal Value { get; set; }
}
