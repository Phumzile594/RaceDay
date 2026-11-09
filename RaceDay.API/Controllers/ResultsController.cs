
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ResultsController(RaceDayContext context)
        {
            _context = context;
        }

        // POST: api/enrolments/1/results
        // Only the event's Organiser can record results.
        [HttpPost("enrolments/{enrolmentId:int}/results")]
        public async Task<IActionResult> RecordResult(
            int enrolmentId,
            [FromBody] RecordResultRequest request)
        {
            var userId =
                HttpContext.Session.GetInt32("UserID");

            var role =
                HttpContext.Session.GetString("Role");

            if (userId == null)
                return Unauthorized("Please log in first.");

            if (role != "Organiser")
                return Forbid();

            if (enrolmentId <= 0)
                return BadRequest("Invalid enrolment ID.");

            // Find the event associated with the enrolment.
            var enrolmentEvent = await (
                from enrolment in _context.Enrolments
                join category in _context.Categories
                    on enrolment.CategoryID equals category.CategoryID
                join raceEvent in _context.Events
                    on category.EventID equals raceEvent.EventID
                where enrolment.EnrolmentID == enrolmentId
                select new
                {
                    enrolment.EnrolmentID,
                    raceEvent.EventID,
                    raceEvent.UserID
                }
            ).AsNoTracking().FirstOrDefaultAsync();

            if (enrolmentEvent == null)
                return NotFound("Enrolment not found.");

            // An Organiser can only record results
            // for events they own.
            if (enrolmentEvent.UserID != userId.Value)
                return Forbid();

            var resultExists = await _context.Results
                .AnyAsync(r => r.EnrolmentID == enrolmentId);

            if (resultExists)
            {
                return Conflict(
                    "A result has already been recorded " +
                    "for this enrolment.");
            }

            if (request.FinishTime <= TimeSpan.Zero)
            {
                return BadRequest(
                    "Finish time must be greater than zero.");
            }

            if (request.Position <= 0)
            {
                return BadRequest(
                    "Finishing position must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(request.ResultStatus))
            {
                return BadRequest(
                    "Result status is required.");
            }

            var result = new Result
            {
                EnrolmentID = enrolmentId,
                FinishTime = request.FinishTime,
                Position = request.Position,
                ResultStatus = request.ResultStatus.Trim(),
                RecordedAt = DateTime.Now
            };

            _context.Results.Add(result);

            await _context.SaveChangesAsync();

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    result.ResultID,
                    result.EnrolmentID,
                    result.FinishTime,
                    result.Position,
                    result.ResultStatus,
                    result.RecordedAt,
                    Message = "Race result recorded successfully."
                });
        }

        // GET: api/results/my-results
        // Participants can view only their own results.
        [HttpGet("results/my-results")]
        public async Task<IActionResult> GetMyResults()
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

            var results = await (
                from result in _context.Results
                join enrolment in _context.Enrolments
                    on result.EnrolmentID equals enrolment.EnrolmentID
                join category in _context.Categories
                    on enrolment.CategoryID equals category.CategoryID
                join raceEvent in _context.Events
                    on category.EventID equals raceEvent.EventID

                where enrolment.ProfileID == profile.ProfileID

                orderby raceEvent.EventDate descending

                select new
                {
                    result.ResultID,
                    result.EnrolmentID,
                    raceEvent.EventID,
                    raceEvent.EventName,
                    raceEvent.EventDate,
                    raceEvent.Location,
                    category.CategoryName,
                    category.DistanceKm,
                    result.FinishTime,
                    result.Position,
                    result.ResultStatus,
                    result.RecordedAt
                }
            )
            .AsNoTracking()
            .ToListAsync();

            return Ok(results);
        }

        // Request model for Organisers recording results.
        public class RecordResultRequest
        {
            [Required]
            public TimeSpan FinishTime { get; set; }

            [Range(1, int.MaxValue)]
            public int Position { get; set; }

            [Required]
            [StringLength(50)]
            public string ResultStatus { get; set; } = "";
        }
    }
}
