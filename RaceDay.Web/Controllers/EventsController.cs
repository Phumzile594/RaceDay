
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EventsController> _logger;

        public EventsController(
            IHttpClientFactory httpClientFactory,
            ILogger<EventsController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory
                    .CreateClient("RaceDayApi");

                _logger.LogInformation(
                    "Requesting events from {ApiAddress}",
                    client.BaseAddress);

                var events = await client.GetFromJsonAsync<
                    List<RaceEventViewModel>>("api/events");

                return View(events ?? new List<RaceEventViewModel>());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex,
                    "Unable to connect to the RaceDay API.");

                ViewBag.ErrorMessage =
                    "Unable to load events. " + ex.Message;

                return View(new List<RaceEventViewModel>());
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex,
                    "Unable to read event data from the RaceDay API.");

                ViewBag.ErrorMessage =
                    "The API returned event data that could not be read.";

                return View(new List<RaceEventViewModel>());
            }
        }
    }

    public class RaceEventViewModel
    {
        public int EventID { get; set; }
        public string EventName { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = "";
        public decimal DistanceKm { get; set; }
        public string EventType { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
