
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using System.Data;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EnrolmentsController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/events/1/categories
        [HttpGet("events/{eventId:int}/categories")]
        public async Task<IActionResult> GetEventCategories(
            int eventId)
        {
            var eventExists = await _context.Events
                .AsNoTracking()
                .AnyAsync(e => e.EventID == eventId);

            if (!eventExists)
                return NotFound("Event not found.");

            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.EventID == eventId)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return Ok(categories);
        }

        // POST: api/events/1/enrolments
        [HttpPost("events/{eventId:int}/enrolments")]
        public async Task<IActionResult> CreateEnrolment(
            int eventId,
            [FromBody] CreateEnrolmentRequest request)
        {
            var userId =
                HttpContext.Session.GetInt32("UserID");

            var role =
                HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized("Please log in first.");

            if (role != "Participant")
                return Forbid();

            if (eventId <= 0 || request.CategoryID <= 0)
            {
                return BadRequest(
                    "Please select a valid event category.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            var category = await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.CategoryID == request.CategoryID &&
                    c.EventID == eventId);

            if (category == null)
            {
                return BadRequest(
                    "The selected category does not belong " +
                    "to this event.");
            }

            // Find the profile belonging to the logged-in user.
            var profile = await _context.Profile
                .FirstOrDefaultAsync(p =>
                    p.UserID == userId.Value);

            // Create a profile only when one does not exist.
            if (profile == null)
            {
                if (string.IsNullOrWhiteSpace(
                        request.PhoneNumber) ||
                    request.DateOfBirth == null ||
                    string.IsNullOrWhiteSpace(
                        request.EmergencyContactName) ||
                    string.IsNullOrWhiteSpace(
                        request.EmergencyContactPhone))
                {
                    return BadRequest(
                        "Please provide your phone number, " +
                        "date of birth and emergency contact " +
                        "details before enrolling.");
                }

                if (request.DateOfBirth.Value.Date >
                    DateTime.Today)
                {
                    return BadRequest(
                        "Date of birth cannot be in the future.");
                }

                profile = new Profile
                {
                    UserID = userId.Value,

                    PhoneNumber =
                        request.PhoneNumber.Trim(),

                    DateOfBirth =
                        request.DateOfBirth.Value.Date,

                    EmergencyContactName =
                        request.EmergencyContactName.Trim(),

                    EmergencyContactPhone =
                        request.EmergencyContactPhone.Trim()
                };

                _context.Profile.Add(profile);

                await _context.SaveChangesAsync();
            }

            // Prevent duplicate enrolment.
            var alreadyEnrolled = await _context.Enrolments
                .AnyAsync(e =>
                    e.ProfileID == profile.ProfileID &&
                    e.CategoryID == request.CategoryID);

            if (alreadyEnrolled)
            {
                return Conflict(
                    "You are already enrolled in this category.");
            }

            // Check category capacity.
            var enrolmentCount = await _context.Enrolments
                .CountAsync(e =>
                    e.CategoryID == request.CategoryID &&
                    e.Status != "Cancelled");

            if (category.MaximumParticipants <= 0 ||
                enrolmentCount >= category.MaximumParticipants)
            {
                return Conflict(
                    "This category is full or unavailable.");
            }

            // Create the enrolment.
            var enrolment = new Enrolment
            {
                ProfileID = profile.ProfileID,
                CategoryID = request.CategoryID,
                EnrolmentDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Enrolments.Add(enrolment);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    enrolment.EnrolmentID,
                    enrolment.ProfileID,
                    enrolment.CategoryID,
                    enrolment.EnrolmentDate,
                    enrolment.Status,
                    Message = "Enrolment created successfully."
                });
        }

        // GET: api/enrolments/my-enrolments
        [HttpGet("enrolments/my-enrolments")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userId =
                HttpContext.Session.GetInt32("UserID");

            var role =
                HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized("Please log in first.");

            if (role != "Participant")
                return Forbid();

            var profile = await _context.Profile
                .AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.UserID == userId.Value);

            if (profile == null)
                return Ok(Array.Empty<object>());

            var enrolments = await (
                from enrolment in _context.Enrolments

                join category in _context.Categories
                    on enrolment.CategoryID equals category.CategoryID

                join raceEvent in _context.Events
                    on category.EventID equals raceEvent.EventID

                where enrolment.ProfileID == profile.ProfileID

                orderby raceEvent.EventDate

                select new
                {
                    enrolment.EnrolmentID,
                    raceEvent.EventID,
                    raceEvent.EventName,
                    raceEvent.EventDate,
                    raceEvent.Location,
                    category.CategoryID,
                    category.CategoryName,
                    category.DistanceKm,
                    category.EntryFee,
                    enrolment.EnrolmentDate,
                    enrolment.Status
                }
            )
            .AsNoTracking()
            .ToListAsync();

            return Ok(enrolments);
        }

        // Request model for Participant enrolment.
        public class CreateEnrolmentRequest
        {
            public int CategoryID { get; set; }

            public string? PhoneNumber { get; set; }

            public DateTime? DateOfBirth { get; set; }

            public string? EmergencyContactName { get; set; }

            public string? EmergencyContactPhone { get; set; }
        }
    }
}
