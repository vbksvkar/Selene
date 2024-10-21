using Models.Reddit;

namespace Clients.Interfaces
{
    public interface IAuthClient
    {
        Task<Token> GetTokenAsync();
    }
}