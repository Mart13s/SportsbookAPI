using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsbookAPI.Adapters;
using SportsbookAPI.Models;

namespace SportsbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventAdapter _eventAdapter;

        public EventsController(IEventAdapter eventAdapter)
        {
            _eventAdapter = eventAdapter;
        }

        [HttpGet(Name = "GetAllEvents")]
        public IActionResult Get()
        {
            List<Event> events;
            
            try
            {
                events = _eventAdapter.GetEvents().ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(events);
        }
    }
}
