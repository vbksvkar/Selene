using Api.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Api.Tests.Infrastructure
{
    public class UnhandledExceptionHandlerTests
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IExceptionHandlerFeature> mockExceptionHandlerFeature;

        public UnhandledExceptionHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockExceptionHandlerFeature = this.mockRepository.Create<IExceptionHandlerFeature>();
        }

        [Theory]
        [InlineData("Test exception")]
        public void OnExceptionAsync_ShouldReturnInternalServerError(string exceptionMessage)
        {
            HttpContext context = new DefaultHttpContext();
            using var stream = new MemoryStream();
            context.Response.Body = stream;

            this.mockExceptionHandlerFeature.SetupGet(x => x.Error).Returns(new Exception(exceptionMessage));
            context.Features.Set(this.mockExceptionHandlerFeature.Object);

            UnhandledExceptionHandler unhandledExceptionHandler = new UnhandledExceptionHandler();
            Task.Run(async () => await unhandledExceptionHandler.OnUnhandledException(context)).Wait();

            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            this.mockRepository.VerifyAll();
        }
    }
}