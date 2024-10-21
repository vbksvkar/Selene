using BusinessLogic.Concretes;
using BusinessLogic.Interfaces;
using Castle.Core.Logging;
using DataStore.Provider;
using Microsoft.Extensions.Logging;
using Models.Reddit;
using Models.Response;
using Moq;

namespace BusinessLogic.Tests.Concretes
{
    public class RedditServiceTests
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IPostsStoreProvider> mockPostsStoreProvider;

        public RedditServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockPostsStoreProvider = this.mockRepository.Create<IPostsStoreProvider>();
        }

        [Fact]
        public async Task ShouldReturnStatsForValidRequest()
        {
            this.mockPostsStoreProvider.Setup(x => x.GetStats(It.IsAny<string>()))
                .Returns(new List<PostsStatsModel>
                {
                    new PostsStatsModel
                    {
                        SubRedditName = "test",
                        StatsText = "Top Posts for test",
                        PostsStats = new List<Tuple<string, int>>
                        {
                            new Tuple<string, int>("Post Title One", 1)
                        }
                    }
                });
            
            RedditService redditService = new RedditService(
                new Mock<ILogger<IRedditService>>().Object,
                this.mockPostsStoreProvider.Object);

            var statsResponse = await redditService.GetStats("test");
            Assert.NotNull(statsResponse);
            Assert.Equal(200, statsResponse.StatusCode);
            Assert.Equal("test", statsResponse.SubredditName);
            Assert.Single(statsResponse.StatsDetails);
            Assert.Equal("Top Posts for test", statsResponse.StatsDetails.First().StatsText);
            Assert.Single(statsResponse.StatsDetails.First().StatsItems);
            Assert.Equal("Post Title One", statsResponse.StatsDetails.First().StatsItems.First().StatValue);
            Assert.Equal(1, statsResponse.StatsDetails.First().StatsItems.First().Count);
        }

        [Fact]
        public async Task ShouldReturnNullForInvalidRequest()
        {
            this.mockPostsStoreProvider.Setup(x => x.GetStats(It.IsAny<string>()))
                .Returns(new List<PostsStatsModel>());
            
            RedditService redditService = new RedditService(
                new Mock<ILogger<IRedditService>>().Object,
                this.mockPostsStoreProvider.Object);
            var statsResponse = await redditService.GetStats("test");
            Assert.NotNull(statsResponse);
            Assert.Equal(404, statsResponse.StatusCode);
            Assert.Equal("test", statsResponse.SubredditName);
            Assert.Single(statsResponse.StatsDetails);
            Assert.Equal("No stats found", statsResponse.StatsDetails.First().StatsText);
            Assert.Empty(statsResponse.StatsDetails.First().StatsItems);
        }
    }
}