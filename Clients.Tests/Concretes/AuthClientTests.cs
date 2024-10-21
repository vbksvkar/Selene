using System.Net;
using Clients.Concretes;
using Clients.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configuration;
using Models.Reddit;
using Moq;
using Moq.Contrib.HttpClient;

namespace Clients.Tests.Concretes
{
    public class AuthClientTests
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IOptions<SeleneConfig>> mockOptionsSeleneConfig;

        public AuthClientTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockOptionsSeleneConfig = this.mockRepository.Create<IOptions<SeleneConfig>>();
        }

        [Fact]
        public async Task ShouldReturnTokenForValidRequest()
        {
            SeleneConfig config = new SeleneConfig
            {
                ClientId = "ClientId", ClientSecret = "ClientSecret", 
                AuthUrl = "https://www.reddit.com/api/v1/access_token",
                UserAgent = "UserAgent"
            };
            this.mockOptionsSeleneConfig.Setup(x => x.Value).Returns(config);

            Mock<HttpMessageHandler> mockHttpMessageHandler = this.mockRepository.Create<HttpMessageHandler>();
            mockHttpMessageHandler.SetupRequest(HttpMethod.Post, config.AuthUrl)
                .ReturnsJsonResponse<Token>(HttpStatusCode.OK, new Token { AccessToken = "token" });                

            HttpClient httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var authClient = new AuthClient(
                httpClient, 
                mockOptionsSeleneConfig.Object, 
                new Mock<ILogger<IAuthClient>>().Object);
            var response = await authClient.GetTokenAsync();
            Assert.NotNull(response);
            Assert.Equal("token", response.AccessToken);
        }

        [Fact]
        public async Task ShouldThrowExceptionForInvalidRequest()
        {
            SeleneConfig config = new SeleneConfig
            {
                ClientId = "ClientId", ClientSecret = "ClientSecret", 
                AuthUrl = "https://www.reddit.com/api/v1/access_token",                
            };
            this.mockOptionsSeleneConfig.Setup(x => x.Value).Returns(config);

            Mock<HttpMessageHandler> mockHttpMessageHandler = this.mockRepository.Create<HttpMessageHandler>();
            mockHttpMessageHandler.SetupRequest(HttpMethod.Post, config.AuthUrl)
                .ReturnsResponse(HttpStatusCode.BadRequest);

            HttpClient httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var authClient = new AuthClient(
                httpClient, 
                mockOptionsSeleneConfig.Object, 
                new Mock<ILogger<IAuthClient>>().Object);

            var action = () => authClient.GetTokenAsync();
            var exception = await Assert.ThrowsAsync<Exception>(action);
            Assert.Equal("Error while fetching token ", exception.Message);
        }
    }
}