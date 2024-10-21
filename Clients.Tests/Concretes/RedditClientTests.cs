using System.Net;
using System.Text.Json;
using Castle.Core.Logging;
using Clients.Concretes;
using Clients.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configuration;
using Models.Reddit;
using Moq;
using Moq.Protected;

namespace Clients.Tests.Concretes
{
    public class RedditClientTests
    {
        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;
        private readonly Mock<ILogger<IRedditClient>> mockLogger;
        private readonly Mock<IOptions<SeleneConfig>> mockOptionsSeleneConfig;
        private readonly SeleneConfig seleneConfig;
        private readonly HttpClient httpClient;

        public RedditClientTests()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            this.httpClient = new HttpClient(mockHttpMessageHandler.Object);
            this.mockLogger = new Mock<ILogger<IRedditClient>>();
            this.mockOptionsSeleneConfig = new Mock<IOptions<SeleneConfig>>();

            seleneConfig = new SeleneConfig
            {
                RedditUrl = "https://reddit.com",
                SubredditConfig = new List<SubredditConfig>
                {
                    new SubredditConfig { Name = "testsubreddit", Interval = "day", PostsCount = 5 }
                }
            };
            mockOptionsSeleneConfig.Setup(o => o.Value).Returns(seleneConfig);
        }

        [Fact]
        public async Task GetPostsAsync_ValidResponse_ReturnsPosts()
        {
            var responseContent = new RedditResponse
            {
                RedditData = new RedditData
                {
                    Children = new List<RedditPostChild>
                    {
                        new RedditPostChild
                        {
                            RedditPost = new RedditPost
                            {
                                SubRedditName = "testsubreddit"
                            }
                        }
                    }
                }
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseContent))
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);
            var redditClient = new RedditClient(httpClient, mockLogger.Object, mockOptionsSeleneConfig.Object);
            var result = await redditClient.GetPostsAsync();

            Assert.Single(result);
            Assert.Equal("testsubreddit", result.First().SubRedditName);
        }
    }
}