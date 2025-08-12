using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.DataContext;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.Helper;
using PharmacyBackend.Models;
using PharmacyBackend.Models.CashierViewModel;
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
        public async Task<int> CreateEmployeeAsync(Employee request)
        //employee
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
            return request.Id;
        }

        public async Task<string> AuthenticationAsync(LogInRequest request)
        {
            var account = await _context.Accounts.Where(a => a.Username.Equals(request.Username))
                .Include(a => a.Employee)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync();
            if (account is null)
                return null;
            if (!account.EncryptedPassword.Equals(request.Password))
                return null;
            string token = _jwtHelper.TokenGeneration(account);
            return token;
        }

        public async Task<bool> CreateAccountAsync(Account request)
        {
            await _context.Accounts.AddAsync(request);
            await _context.SaveChangesAsync();
            return true;
        }
        //sale
        public async Task<SalesTransaction> CreateSalesTransactionAsync(SalesTransaction sale)
        {
            await _context.SalesTransaction.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        //Discount
        public async Task<Discount> CreateDiscountAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        //Product
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
        //Category
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        //ItemDataControl

        //Brand
        public async Task<Brand> CreateBrandAsync(Brand brand)
        {
            await _context.Brand.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        //Generic 
        public async Task<Generic> CreateGenericAsync(Generic generic)
        {
            await _context.Generic.AddAsync(generic);
            await _context.SaveChangesAsync();
            return generic;
        }

        //Formulation

        public async Task<Formulation> CreateFormulationAsync(Formulation formulation)
        {
            await _context.Formulation.AddAsync(formulation);
            await _context.SaveChangesAsync();
            return formulation;
        }

        //Classification
        public async Task<Classification> CreateClassificationAsync(Classification classification)
        {
            await _context.Classification.AddAsync(classification);
            await _context.SaveChangesAsync();
            return classification;
        }

        //Type
        public async Task<TypeModel> CreateTypeModelAsync(TypeModel typeModel)
        {
            await _context.TypeModel.AddAsync(typeModel);
            await _context.SaveChangesAsync();
            return typeModel;
        }

    }
}
