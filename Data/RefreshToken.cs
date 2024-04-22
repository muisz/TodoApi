using System.ComponentModel.DataAnnotations;

namespace TodoApi.Data
{
    public class RefreshToken
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}