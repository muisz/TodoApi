using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITokenService
    {
        public Token CreatePairToken(User user);
        public string CreateAccessToken(User user);
        public string CreateRefreshToken(User user);
        public Token Refresh(string token);
    }
}