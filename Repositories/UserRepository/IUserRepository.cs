using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IUserRepository
    {
        public Task<User> Create(User user);
        public Task<User?> GetFromEmail(string value);
    }
}