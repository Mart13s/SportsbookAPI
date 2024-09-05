using Newtonsoft.Json;
using SportsbookAPI.Models;

namespace SportsbookAPI.Adapters;
public class PlayerJsonAdapter : IPlayerAdapter
{
    private readonly string _filePath = "./Data/Player.json";

    public PlayerJsonAdapter(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<Player> GetPlayers()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"File not found {_filePath}.");
        }

        var jsonData = File.ReadAllText(_filePath);
        if (jsonData == null) return new List<Player>();

        IEnumerable<Player> players;

        try
        {
            players = JsonConvert.DeserializeObject<IEnumerable<Player>>(jsonData) ?? new List<Player>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while reading players: {ex.Message}.");
        }

        return players;
    }
}