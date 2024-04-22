using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Exceptions;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [Route("/api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Auth>> PostRegister(RegisterUser user)
        {
            try
            {
                User createdUser = await _userService.Register(user);
                Token token = _tokenService.CreatePairToken(createdUser);
                return Ok(new Auth
                {
                    Id = createdUser.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    Token = token,
                });
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Auth>> PostLogin(LoginUser payload)
        {
            try
            {
                User user = await _userService.Authenticate(payload.Email, payload.Password);
                Token token = _tokenService.CreatePairToken(user);
                return Ok(new Auth
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Token = token,
                });
            }
            catch (HttpException error)
            {
                return Problem(error.Message, statusCode: error.StatusCode);
            }
        }

        [HttpPost("token/refresh")]
        public Task<ActionResult<Token>> PostRefreshToken(RefreshToken payload)
        {
            try
            {
                Token newToken = _tokenService.Refresh(payload.Token);
                return Task.FromResult<ActionResult<Token>>(Ok(newToken));
            }
            catch (HttpException error)
            {
                return Task.FromResult<ActionResult<Token>>(Problem(error.Message, statusCode: error.StatusCode));
            }
            catch (Exception)
            {
                return Task.FromResult<ActionResult<Token>>(Problem("invalid token", statusCode: StatusCodes.Status400BadRequest));
            }
        }
    }
}