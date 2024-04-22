using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }
    }
}