using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("/api/v1/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ITokenService _tokenService;

        public TodoController(ITodoService todoService, ITokenService tokenService)
        {
            _todoService = todoService;
            _tokenService = tokenService;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<ActionResult<TodoItem>> PostCreateTodo(CreateTodo todo)
        {
            try
            {
                int userId = _tokenService.GetIdentifier(User);
                Todo newTodo = await _todoService.CreateTodo(todo, userId);
                return Ok(new TodoItem
                {
                    Id = newTodo.Id,
                    Name = newTodo.Name,
                    IsCompleted = newTodo.IsCompleted,
                    CompletedAt = newTodo.CompletedAt,
                    CreatedAt = newTodo.CreatedAt,
                });
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("")]
        [Authorize]
        public async Task<ActionResult<List<TodoItem>>> GetTodos()
        {
            try
            {
                int userId = _tokenService.GetIdentifier(User);
                List<Todo> todos = await _todoService.GetTodosFromUser(userId);
                List<TodoItem> items = new List<TodoItem>();
                todos.ForEach(todo => {
                    items.Add(new TodoItem
                    {
                        Id = todo.Id,
                        Name = todo.Name,
                        IsCompleted = todo.IsCompleted,
                        CompletedAt = todo.CompletedAt,
                        CreatedAt = todo.CreatedAt,
                    });
                });
                return Ok(items);
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoItem>> GetTodo(int id)
        {
            try
            {
                int userId = _tokenService.GetIdentifier(User);
                Todo? todo = await _todoService.GetTodo(id);
                if (todo == null)
                    throw new HttpException("not found", StatusCodes.Status404NotFound);
                
                TodoItem item = new TodoItem
                {
                    Id = todo.Id,
                    Name = todo.Name,
                    IsCompleted = todo.IsCompleted,
                    CompletedAt = todo.CompletedAt,
                    CreatedAt = todo.CreatedAt,
                };
                return Ok(item);
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpPost("{id}/completed")]
        [Authorize]
        public async Task<ActionResult<TodoItem>> PutTodo(int id)
        {
            try
            {
                int userId = _tokenService.GetIdentifier(User);
                Todo? todo = await _todoService.GetTodo(id);
                if (todo == null)
                    throw new HttpException("not found", StatusCodes.Status404NotFound);
                
                todo = await _todoService.MarkAsCompleted(todo);
                TodoItem item = new TodoItem
                {
                    Id = todo.Id,
                    Name = todo.Name,
                    IsCompleted = todo.IsCompleted,
                    CompletedAt = todo.CompletedAt,
                    CreatedAt = todo.CreatedAt,
                };
                return Ok(item);
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            try
            {
                int userId = _tokenService.GetIdentifier(User);
                Todo? todo = await _todoService.GetTodo(id);
                if (todo == null)
                    throw new HttpException("not found", StatusCodes.Status404NotFound);
                
                await _todoService.DeleteTodo(id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }
    }
}