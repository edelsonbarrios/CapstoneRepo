using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.Models;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmacyBackend.Repository.POST
{
    public class PostRepository : IPostRepositoryContracts
    {
        private readonly PharmacyContext _context;
        private readonly IConfiguration _config;
        public PostRepository(PharmacyContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string TokenGeneration(Account request)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, request.Employee.Role.ToString()),
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

        public async Task<bool> CreateEmployeeAsync(Employee request)
        {
            await _context.Employees.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AuthenticationAsync(LogInRequest request)
        {
            var account = await _context.Accounts.Where(a => a.Username.Equals(request.Username))
                .Include(a => a.Employee)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync();
            if (account is null)
                return false;
            if (!account.EncryptedPassword.Equals(request.Password))
                return false;
            return true;
        }
    }
}
