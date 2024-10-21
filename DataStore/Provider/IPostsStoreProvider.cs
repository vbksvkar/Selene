using Models.Reddit;

namespace DataStore.Provider
{
    public interface IPostsStoreProvider
    {
        void AddStats(List<PostsModel> posts);
        List<PostsStatsModel> GetStats(string subredditName);
    }
}