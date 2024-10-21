using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;

namespace Api.Tests
{
    public class RedditControllerTests
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IRedditService> mockRedditService;

        public RedditControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockRedditService = this.mockRepository.Create<IRedditService>();            
        }

        [Fact]
        public async Task ShouldReturn200ForValidRequest()
        {
            RedditController redditController = new RedditController(this.mockRedditService.Object);
            var actionContext = new ActionContext(
                new DefaultHttpContext { Request = { Method = HttpMethods.Get } },
                new Microsoft.AspNetCore.Routing.RouteData(),
                new ControllerActionDescriptor());

            this.mockRedditService.Setup(x => x.GetStats(It.IsAny<string>()))
                .ReturnsAsync(new Models.Response.StatsResponse
                {
                    StatusCode = 200,
                });

            redditController.ControllerContext = new ControllerContext(actionContext);
            var result = await redditController.GetStats("test");
            Assert.Equal(StatusCodes.Status200OK, (result.Result as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task ShouldReturn404ForInvalidRequest()
        {
            RedditController redditController = new RedditController(this.mockRedditService.Object);
            var actionContext = new ActionContext(
                new DefaultHttpContext { Request = { Method = HttpMethods.Get } },
                new Microsoft.AspNetCore.Routing.RouteData(),
                new ControllerActionDescriptor());

            this.mockRedditService.Setup(x => x.GetStats(It.IsAny<string>()))
                .ReturnsAsync(new Models.Response.StatsResponse
                {
                    StatusCode = 404,
                });

            redditController.ControllerContext = new ControllerContext(actionContext);
            var result = await redditController.GetStats("zxzz");
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, (result.Result as NotFoundResult).StatusCode);            
        }
    }
}