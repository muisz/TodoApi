using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface IUserService
    {
        public Task<User> Register(RegisterUser user);
        public Task<User> Authenticate(string email, string password);
    }
}