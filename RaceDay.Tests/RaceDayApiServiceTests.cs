
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Services;
using Xunit;

namespace RaceDay.Tests
{
    public class RaceDayApiServiceTests
    {
        [Fact]
        public void CreateAuthenticatedClient_WithoutLogin_Throws()
        {
            // Arrange
            var accessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };

            var factory = new TestHttpClientFactory();

            var service = new RaceDayApiService(
                factory, accessor);

            // Act and Assert
            Assert.Throws<UnauthorizedAccessException>(
                () => service.CreateAuthenticatedClient());
        }

        private class TestHttpClientFactory : IHttpClientFactory
        {
            public HttpClient CreateClient(string name)
            {
                return new HttpClient();
            }
        }
    }
}
