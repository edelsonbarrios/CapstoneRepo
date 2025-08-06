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
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace PharmacyBackend.Repository.POST
{
    public class PostRepository : IPostRepositoryContracts
    {
        private readonly PharmacyContext _context;
        private readonly IConfiguration _config;
        private readonly JwtHelper _jwtHelper;
        public PostRepository(PharmacyContext context,
            IConfiguration config,
            JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _config = config;
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
            string token = _jwtHelper.TokenGeneration(account);
            return true;
        }

        public async Task<bool> CreateAccountAsync(Account request)
        {
            await _context.Accounts.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
