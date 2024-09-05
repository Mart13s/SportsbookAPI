using Newtonsoft.Json;
using SportsbookAPI.Models;

namespace SportsbookAPI.Adapters;
public class EventJsonAdapter : IEventAdapter
{
    private readonly string _filePath = "./Data/Events.json";

    public EventJsonAdapter(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<Event> GetEvents()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"File not found {_filePath}.");
        }

        var jsonData = File.ReadAllText(_filePath);
        if (jsonData == null) return new List<Event>();

        IEnumerable<Event> events;

        try
        {
            events = JsonConvert.DeserializeObject<IEnumerable<Event>>(jsonData) ?? new List<Event>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while reading events: {ex.Message}.");
        }

        return events;
    }
}