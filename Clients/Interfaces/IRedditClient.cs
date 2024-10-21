using Models.Reddit;

namespace Clients.Interfaces
{
    public interface IRedditClient
    {
        Task<List<PostsModel>> GetPostsAsync();
    }
}