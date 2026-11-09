
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RaceDay.Web.Services;
using System.Net;
using System.Net.Http.Json;

namespace RaceDay.Web.Controllers
{
    [Authorize(Roles = "Participant")]
    public class ParticipantController : Controller
    {
        private readonly RaceDayApiService? _apiService;

        // Constructor used by ASP.NET Core MVC.
        [ActivatorUtilitiesConstructor]
        public ParticipantController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        // Kept for existing dashboard unit tests.
        public ParticipantController()
        {
        }

        private HttpClient GetApiClient()
        {
            if (_apiService == null)
            {
                throw new InvalidOperationException(
                    "RaceDay API service is not configured.");
            }

            return _apiService.CreateAuthenticatedClient();
        }

        // GET: /Participant
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.WelcomeMessage =
                "Welcome to your RaceDay Participant Dashboard!";

            return View();
        }

        // GET: /Participant/Enrol?eventId=1
        [HttpGet]
        public async Task<IActionResult> Enrol(int eventId)
        {
            if (eventId <= 0)
                return BadRequest("Invalid event ID.");

            try
            {
                using var client = GetApiClient();

                using var eventResponse =
                    await client.GetAsync($"api/events/{eventId}");

                if (eventResponse.StatusCode == HttpStatusCode.NotFound)
                    return NotFound("Event not found.");

                if (eventResponse.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (!eventResponse.IsSuccessStatusCode)
                {
                    return StatusCode(
                        (int)eventResponse.StatusCode,
                        "Unable to load event details.");
                }

                var raceEvent = await eventResponse.Content
                    .ReadFromJsonAsync<RaceEventInfo>();

                using var categoryResponse =
                    await client.GetAsync(
                        $"api/events/{eventId}/categories");

                if (categoryResponse.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (!categoryResponse.IsSuccessStatusCode)
                {
                    return StatusCode(
                        (int)categoryResponse.StatusCode,
                        "Unable to load event categories.");
                }

                var categories = await categoryResponse.Content
                    .ReadFromJsonAsync<List<CategoryInfo>>();

                var model = new EventEnrolmentViewModel
                {
                    EventID = eventId,
                    EventName = raceEvent?.EventName ?? "RaceDay Event",
                    Categories = categories ?? new List<CategoryInfo>()
                };

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503,
                    "Unable to connect to RaceDay API.");
            }
        }

        // POST: /Participant/Enrol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(
            int eventId,
            int categoryId,
            string? phoneNumber,
            DateTime? dateOfBirth,
            string? emergencyContactName,
            string? emergencyContactPhone)
        {
            if (eventId <= 0 || categoryId <= 0)
            {
                TempData["ErrorMessage"] =
                    "Please select a valid event category.";

                return RedirectToAction(
                    nameof(Enrol),
                    new { eventId });
            }

            try
            {
                using var client = GetApiClient();

                using var response = await client.PostAsJsonAsync(
                    $"api/events/{eventId}/enrolments",
                    new
                    {
                        CategoryID = categoryId,
                        PhoneNumber = phoneNumber,
                        DateOfBirth = dateOfBirth,
                        EmergencyContactName = emergencyContactName,
                        EmergencyContactPhone = emergencyContactPhone
                    });

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    return Forbid();

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "You have successfully enrolled in the event!";

                    return RedirectToAction(nameof(MyEnrolments));
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ErrorMessage"] =
                        "You may already be enrolled, " +
                        "or this category is full.";
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["ErrorMessage"] =
                        "Please complete your profile details " +
                        "and select a valid category.";
                }
                else
                {
                    TempData["ErrorMessage"] =
                        "Unable to complete enrolment. Please try again.";
                }

                return RedirectToAction(
                    nameof(Enrol),
                    new { eventId });
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503,
                    "Unable to connect to RaceDay API.");
            }
        }

        // GET: /Participant/MyEnrolments
        [HttpGet]
        public async Task<IActionResult> MyEnrolments()
        {
            try
            {
                using var client = GetApiClient();

                using var response = await client.GetAsync(
                    "api/enrolments/my-enrolments");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    return Forbid();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(
                        (int)response.StatusCode,
                        "Unable to retrieve your enrolments.");
                }

                var enrolments = await response.Content
                    .ReadFromJsonAsync<List<MyEnrolmentViewModel>>();

                return View(
                    enrolments ?? new List<MyEnrolmentViewModel>());
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503,
                    "Unable to connect to RaceDay API.");
            }
        }

        // GET: /Participant/MyResults
        [HttpGet]
        public async Task<IActionResult> MyResults()
        {
            try
            {
                using var client = GetApiClient();

                using var response = await client.GetAsync(
                    "api/results/my-results");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    return Forbid();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(
                        (int)response.StatusCode,
                        "Unable to retrieve your race results.");
                }

                var results = await response.Content
                    .ReadFromJsonAsync<List<MyResultViewModel>>();

                return View(
                    results ?? new List<MyResultViewModel>());
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503,
                    "Unable to connect to RaceDay API.");
            }
        }

        // Event information returned by RaceDay.API.
        public class RaceEventInfo
        {
            public int EventID { get; set; }
            public string EventName { get; set; } = "";
        }

        // Category information returned by RaceDay.API.
        public class CategoryInfo
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; } = "";
            public decimal DistanceKm { get; set; }
            public int MaximumParticipants { get; set; }
            public decimal EntryFee { get; set; }
        }

        // View model for Event Enrolment.
        public class EventEnrolmentViewModel
        {
            public int EventID { get; set; }
            public string EventName { get; set; } = "";

            public List<CategoryInfo> Categories { get; set; }
                = new List<CategoryInfo>();
        }

        // View model for My Enrolments.
        public class MyEnrolmentViewModel
        {
            public int EnrolmentID { get; set; }
            public int EventID { get; set; }
            public string EventName { get; set; } = "";
            public DateTime EventDate { get; set; }
            public string Location { get; set; } = "";
            public int CategoryID { get; set; }
            public string CategoryName { get; set; } = "";
            public decimal DistanceKm { get; set; }
            public decimal EntryFee { get; set; }
            public DateTime EnrolmentDate { get; set; }
            public string Status { get; set; } = "";
        }

        // View model for My Results.
        public class MyResultViewModel
        {
            public int ResultID { get; set; }
            public int EnrolmentID { get; set; }
            public int EventID { get; set; }
            public string EventName { get; set; } = "";
            public DateTime EventDate { get; set; }
            public string Location { get; set; } = "";
            public string CategoryName { get; set; } = "";
            public decimal DistanceKm { get; set; }
            public TimeSpan FinishTime { get; set; }
            public int Position { get; set; }
            public string ResultStatus { get; set; } = "";
            public DateTime RecordedAt { get; set; }
        }
    }
}
