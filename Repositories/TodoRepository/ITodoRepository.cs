using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        public Task<Todo> Create(Todo todo);
        public Task<List<Todo>> GetTodosFromUser(int id);
        public Task<Todo?> GetTodo(int id);
        public Task<Todo> Update(Todo todo);
        public Task Delete(int id);
    }
}