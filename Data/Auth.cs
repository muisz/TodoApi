namespace TodoApi.Data
{
    public class Auth
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Token? Token { get; set; }
    }
}