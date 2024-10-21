using Models.Response;

namespace BusinessLogic.Interfaces
{
    public interface IRedditService
    {
        Task<StatsResponse> GetStats(string subRedditName);
    }
}