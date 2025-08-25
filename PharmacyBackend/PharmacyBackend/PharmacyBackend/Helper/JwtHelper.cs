using Microsoft.IdentityModel.Tokens;
using PharmacyBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmacyBackend.Helper
{
    public  class JwtHelper
    {
        private readonly IConfiguration _config;
        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }
        public string TokenGeneration(Account request)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, request.Employee.Role.RolePosition),
                new Claim("EmployeeId", request.Employee.Id.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:SecretKey")));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

            var tokenDescription = new JwtSecurityToken(
                issuer: _config.GetValue<string>("Jwt:Issuer"),
                audience: _config.GetValue<string>("Jwt:Audience"),
                expires: DateTime.UtcNow.AddHours(_config.GetValue<int>("Jwt:TokenValidity")),
                claims: claims,
                signingCredentials: signingCreds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);

        }
    }
}
