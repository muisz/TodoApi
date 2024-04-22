using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Todo> Create(Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<List<Todo>> GetTodosFromUser(int id)
        {
            return await _context.Todos
                .Where(todo => todo.UserId == id)
                .ToListAsync();
        }

        public async Task<Todo?> GetTodo(int id)
        {
            return await _context.Todos.SingleOrDefaultAsync(todo => todo.Id == id);
        }

        public async Task<Todo> Update(Todo todo)
        {
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task Delete(int id)
        {
            await _context.Todos.Where(todo => todo.Id == id).ExecuteDeleteAsync();
        }
    }
}