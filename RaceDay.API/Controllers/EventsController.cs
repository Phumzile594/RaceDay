
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventsController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _context.Events
                .AsNoTracking()
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var raceEvent = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
                return NotFound("Event not found.");

            return Ok(raceEvent);
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> CreateEvent(
            [FromBody] EventRequest request)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized("Please log in.");

            if (role != "Organiser")
                return Forbid();

            var raceEvent = new Event
            {
                UserID = userId.Value,
                EventName = request.EventName.Trim(),
                Description = request.Description,
                EventDate = request.EventDate,
                StartTime = request.StartTime,
                Location = request.Location.Trim(),
                DistanceKm = request.DistanceKm,
                EventType = request.EventType,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Events.Add(raceEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = raceEvent.EventID },
                raceEvent);
        }

        // PUT: api/events/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            [FromBody] EventRequest request)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized("Please log in.");

            if (role != "Organiser")
                return Forbid();

            var raceEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
                return NotFound("Event not found.");

            if (raceEvent.UserID != userId.Value)
                return Forbid();

            raceEvent.EventName = request.EventName.Trim();
            raceEvent.Description = request.Description;
            raceEvent.EventDate = request.EventDate;
            raceEvent.StartTime = request.StartTime;
            raceEvent.Location = request.Location.Trim();
            raceEvent.DistanceKm = request.DistanceKm;
            raceEvent.EventType = request.EventType;
            raceEvent.Status = request.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/events/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized("Please log in.");

            if (role != "Organiser")
                return Forbid();

            var raceEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
                return NotFound("Event not found.");

            if (raceEvent.UserID != userId.Value)
                return Forbid();

            _context.Events.Remove(raceEvent);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "This event may have related enrolments, " +
                    "categories or results and cannot be deleted.");
            }

            return NoContent();
        }

        public class EventRequest
        {
            [Required]
            [StringLength(150, MinimumLength = 3)]
            public string EventName { get; set; } = "";

            public string? Description { get; set; }

            [Required]
            public DateTime EventDate { get; set; }

            public TimeSpan StartTime { get; set; }

            [Required]
            [StringLength(150)]
            public string Location { get; set; } = "";

            [Range(0.01, 1000)]
            public decimal DistanceKm { get; set; }

            [Required]
            public string EventType { get; set; } = "";

            [Required]
            public string Status { get; set; } = "Upcoming";
        }
    }
}
