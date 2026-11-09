
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Controllers;
using RaceDay.Web.Services;
using Xunit;

namespace RaceDay.Tests
{
    public class OrganiserControllerTests
    {
        private OrganiserController CreateController()
        {
            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };

            var httpClientFactory = new TestHttpClientFactory();

            var apiService = new RaceDayApiService(
                httpClientFactory,
                httpContextAccessor);

            return new OrganiserController(apiService);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_SetsWelcomeMessage()
        {
            // Arrange
            var controller = CreateController();

            // Act
            controller.Index();

            // Assert
            Assert.Equal(
                "Welcome to your RaceDay Organiser Dashboard!",
                controller.ViewBag.WelcomeMessage);
        }

        [Fact]
        public void OrganiserController_RequiresOrganiserRole()
        {
            // Act
            var attribute = Attribute.GetCustomAttribute(
                typeof(OrganiserController),
                typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("Organiser", attribute.Roles);
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
