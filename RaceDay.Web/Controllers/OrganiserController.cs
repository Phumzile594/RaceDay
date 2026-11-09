
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;

namespace RaceDay.Web.Controllers
{
    [Authorize(Roles = "Organiser")]
    public class OrganiserController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public OrganiserController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        // ORGANISER DASHBOARD
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.WelcomeMessage =
                "Welcome to your RaceDay Organiser Dashboard!";

            return View();
        }

        // TEMPORARY API SESSION CHECK
        [HttpGet]
        public async Task<IActionResult> CheckApiSession()
        {
            try
            {
                using var client =
                    _apiService.CreateAuthenticatedClient();

                using var response =
                    await client.GetAsync("api/auth/me");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized(
                        "Your API session has expired. Please log in again.");

                if (!response.IsSuccessStatusCode)
                    return StatusCode(
                        (int)response.StatusCode,
                        "Unable to verify your API session.");

                return Content("RaceDay API session is valid.");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Please log in again.");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503, "The RaceDay API is unavailable.");
            }
        }

        // CREATE EVENT - GET
        [HttpGet]
        public IActionResult CreateEvent()
        {
            return View(new EventFormViewModel());
        }

        // CREATE EVENT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(
            EventFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client =
                    _apiService.CreateAuthenticatedClient();

                using var response = await client.PostAsJsonAsync(
                    "api/events",
                    new
                    {
                        model.EventName,
                        model.Description,
                        model.EventDate,
                        model.StartTime,
                        model.Location,
                        model.DistanceKm,
                        model.EventType,
                        model.Status
                    });

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(
                        "", "Your API session has expired. Please log in again.");

                    return View(model);
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    return Forbid();

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Event created successfully!";

                    return RedirectToAction("Index", "Events");
                }

                ModelState.AddModelError(
                    "", "Could not create the event. Please check your details.");
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError(
                    "", "Your session has expired. Please log in again.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    "", "Unable to connect to RaceDay API.");
            }

            return View(model);
        }

        // EDIT EVENT - GET
        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                using var client =
                    _apiService.CreateAuthenticatedClient();

                using var response =
                    await client.GetAsync($"api/events/{id}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return NotFound("Event not found.");

                if (!response.IsSuccessStatusCode)
                    return StatusCode(
                        (int)response.StatusCode,
                        "Unable to load this event.");

                var raceEvent = await response.Content
                    .ReadFromJsonAsync<EventFormViewModel>();

                if (raceEvent == null)
                    return NotFound("Event information was not found.");

                return View(raceEvent);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503, "Unable to connect to RaceDay API.");
            }
        }

        // EDIT EVENT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(
            int id, EventFormViewModel model)
        {
            if (id <= 0 || id != model.EventID)
                return BadRequest("Invalid event ID.");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client =
                    _apiService.CreateAuthenticatedClient();

                using var response = await client.PutAsJsonAsync(
                    $"api/events/{id}",
                    new
                    {
                        model.EventName,
                        model.Description,
                        model.EventDate,
                        model.StartTime,
                        model.Location,
                        model.DistanceKm,
                        model.EventType,
                        model.Status
                    });

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(
                        "", "Your API session has expired. Please log in again.");

                    return View(model);
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    return Forbid();

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return NotFound("Event not found.");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Event updated successfully!";

                    return RedirectToAction("Index", "Events");
                }

                ModelState.AddModelError(
                    "", "Could not update the event. Please check your details.");
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError(
                    "", "Your session has expired. Please log in again.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    "", "Unable to connect to RaceDay API.");
            }

            return View(model);
        }

        // DELETE EVENT - GET
        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                using var client =
                    _apiService.CreateAuthenticatedClient();

                using var response =
                    await client.GetAsync($"api/events/{id}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return NotFound("Event not found.");

                if (!response.IsSuccessStatusCode)
                    return StatusCode(
                        (int)response.StatusCode,
                        "Unable to load this event.");

                var raceEvent = await response.Content
                    .ReadFromJsonAsync<EventFormViewModel>();

                if (raceEvent == null)
                    return NotFound("Event not found.");

                return View(raceEvent);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503, "Unable to connect to RaceDay API.");
            }
        }

        // DELETE EVENT - POST
        [HttpPost]
        [ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEventConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                using var client =
                    _apiService.CreateAuthenticatedClient();

                using var response =
                    await client.DeleteAsync($"api/events/{id}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    return Forbid();

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return NotFound("Event not found.");

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ErrorMessage"] =
                        "This event has related records and cannot be deleted.";

                    return RedirectToAction("Index", "Events");
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Event deleted successfully!";

                    return RedirectToAction("Index", "Events");
                }

                TempData["ErrorMessage"] =
                    "Could not delete the event.";

                return RedirectToAction("Index", "Events");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    503, "Unable to connect to RaceDay API.");
            }
        }

        // RECORD RESULT - GET
        [HttpGet]
        public IActionResult RecordResult()
        {
            return View(new RecordResultViewModel());
        }

        // RECORD RESULT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordResult(
            RecordResultViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using var client =
                    _apiService.CreateAuthenticatedClient();

                using var response = await client.PostAsJsonAsync(
                    $"api/enrolments/{model.EnrolmentID}/results",
                    new
                    {
                        model.FinishTime,
                        model.Position,
                        model.ResultStatus
                    });

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(
                        "", "Your session has expired. Please log in again.");

                    return View(model);
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    ModelState.AddModelError(
                        "",
                        "You can only record results for events you organise.");

                    return View(model);
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError(
                        "", "The enrolment was not found.");

                    return View(model);
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    ModelState.AddModelError(
                        "", "A result already exists for this enrolment.");

                    return View(model);
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] =
                        "Race result recorded successfully!";

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    "", "Unable to record the result. Check your details.");
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError(
                    "", "Please log in again.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    "", "Unable to connect to RaceDay API.");
            }

            return View(model);
        }

        // MODEL FOR RECORD RESULT FORM
        public class RecordResultViewModel
        {
            [Range(1, int.MaxValue,
                ErrorMessage = "Enter a valid enrolment ID.")]
            [Display(Name = "Enrolment ID")]
            public int EnrolmentID { get; set; }

            [Required]
            [Display(Name = "Finish Time")]
            public TimeSpan FinishTime { get; set; }

            [Range(1, int.MaxValue,
                ErrorMessage = "Position must be greater than zero.")]
            public int Position { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "Result Status")]
            public string ResultStatus { get; set; } = "Finished";
        }
    }
}
