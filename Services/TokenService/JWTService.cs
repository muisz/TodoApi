using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class JWTService : ITokenService
    {
        private readonly string _accessKey;
        private readonly string _refreshKey;
        private readonly string _issuer;
        private readonly int _accessHours;
        private readonly int _refreshHours;        

        public JWTService(IConfiguration configuration)
        {
            _accessKey = configuration["JWT:AccessKey"] ?? "";
            _refreshKey = configuration["JWT:RefreshKey"] ?? "";
            _issuer = configuration["JWT:Issuer"] ?? "";
            _accessHours = int.Parse(configuration["JWT:AccessHours"] ?? "0");
            _refreshHours = int.Parse(configuration["JWT:RefreshHours"] ?? "0");
        }

        public Token CreatePairToken(User user)
        {
            return new Token
            {
                Access = CreateAccessToken(user),
                Refresh = CreateRefreshToken(user),
            };
        }

        public string CreateAccessToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };
            return CreateToken(claims, _accessKey, _accessHours);
        }

        public string CreateRefreshToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };
            return CreateToken(claims, _refreshKey, _refreshHours);
        }

        public Token Refresh(string token)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_refreshKey));
            TokenValidationParameters parameters = new TokenValidationParameters
            {
                ValidIssuer = _issuer,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out SecurityToken validatedToken);
            List<Claim> claims = principal.Claims.ToList();
            return new Token
            {
                Access = CreateToken(claims, _accessKey, _accessHours),
                Refresh = CreateToken(claims, _refreshKey, _refreshHours),
            };
        }

        private string CreateToken(List<Claim> claims, string key, int activeHours)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(activeHours),
                signingCredentials: credentials,
                issuer: _issuer
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetIdentifier(ClaimsPrincipal principal)
        {
            string value = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(value);
        }
    }
}