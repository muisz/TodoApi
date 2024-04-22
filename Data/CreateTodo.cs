using System.ComponentModel.DataAnnotations;

namespace TodoApi.Data
{
    public class CreateTodo
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}