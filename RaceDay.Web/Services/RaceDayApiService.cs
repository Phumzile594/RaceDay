
using System.Net.Http.Headers;

namespace RaceDay.Web.Services
{
    public class RaceDayApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceDayApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient CreateAuthenticatedClient()
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException(
                    "No active HTTP context.");

            if (context.User.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException(
                    "You must log in first.");
            }

            var apiCookie = context.Session.GetString(
                "RaceDayApiSessionCookie");

            if (string.IsNullOrWhiteSpace(apiCookie))
            {
                throw new UnauthorizedAccessException(
                    "Your API session has expired. Please log in again.");
            }

            var client = _httpClientFactory.CreateClient("RaceDayApi");

            client.DefaultRequestHeaders.Add("Cookie", apiCookie);

            return client;
        }
    }
}
