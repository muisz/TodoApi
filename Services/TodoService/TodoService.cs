using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Todo> CreateTodo(CreateTodo todo, int userId)
        {
            Todo newTodo = new Todo
            {
                Name = todo.Name,
                IsCompleted = false,
                UserId = userId,
                CreatedAt = DateTime.Now.ToUniversalTime(),
            };
            return await _todoRepository.Create(newTodo);
        }

        public async Task<List<Todo>> GetTodosFromUser(int id)
        {
            return await _todoRepository.GetTodosFromUser(id);
        }

        public async Task<Todo?> GetTodo(int id)
        {
            return await _todoRepository.GetTodo(id);
        }

        public async Task<Todo> MarkAsCompleted(Todo todo)
        {
            todo.IsCompleted = true;
            todo.CompletedAt = DateTime.Now.ToUniversalTime();
            return await _todoRepository.Update(todo);
        }

        public async Task DeleteTodo(int id)
        {
            await _todoRepository.Delete(id);
        }
    }
}