
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Controllers;
using Xunit;

namespace RaceDay.Tests
{
    public class ParticipantControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            var controller = new ParticipantController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_SetsWelcomeMessage()
        {
            var controller = new ParticipantController();

            controller.Index();

            Assert.Equal(
                "Welcome to your RaceDay Participant Dashboard!",
                controller.ViewBag.WelcomeMessage);
        }

        [Fact]
        public void ParticipantController_RequiresParticipantRole()
        {
            var attribute = Attribute.GetCustomAttribute(
                typeof(ParticipantController),
                typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            Assert.NotNull(attribute);
            Assert.Equal("Participant", attribute.Roles);
        }
    }
}
