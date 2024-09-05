using SportsbookAPI.Models;

namespace SportsbookAPI.Adapters
{
    public interface IEventAdapter
    {
        IEnumerable<Event> GetEvents();
    }
}