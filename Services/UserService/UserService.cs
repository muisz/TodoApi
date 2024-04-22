using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public UserService(IUserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task<User> Register(RegisterUser user)
        {
            User? userWithSameEmail = await _userRepository.GetFromEmail(user.Email);
            if (userWithSameEmail != null)
                throw new HttpException("user already exist");

            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email.ToLower(),
                Password = _hasher.Hash(user.Password),
                CreatedAt = DateTime.Now.ToUniversalTime(),
            };
            await _userRepository.Create(newUser);
            return newUser;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            User? user = await _userRepository.GetFromEmail(email);
            if (user == null)
                throw new HttpException("user not found", StatusCodes.Status404NotFound);
            if (!_hasher.Check(password, user.Password))
                throw new HttpException("wrong password");
            return user;
        }
    }
}