namespace TodoApi.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public bool Check(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}