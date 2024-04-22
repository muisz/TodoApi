using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        public Task<Todo> CreateTodo(CreateTodo todo, int userId);
        public Task<List<Todo>> GetTodosFromUser(int id);
        public Task<Todo?> GetTodo(int id);
        public Task<Todo> MarkAsCompleted(Todo todo);
        public Task DeleteTodo(int id);
    }
}