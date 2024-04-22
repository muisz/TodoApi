using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetFromEmail(string value)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == value);
        }
    }
}