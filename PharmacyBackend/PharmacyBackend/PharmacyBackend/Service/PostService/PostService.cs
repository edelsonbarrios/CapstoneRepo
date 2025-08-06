using PharmacyBackend.Contracts.RepositoryContracts.PostInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.Helper;

namespace PharmacyBackend.Service.PostService
{
    public class PostService : IPostServiceContracts
    {
        private readonly IPostRepositoryContracts _postRepo;

        public PostService(IPostRepositoryContracts postRepo)
        {
            _postRepo = postRepo;
        }

        public async Task<string> AuthenticationAsync(LogInRequest request)
        {
            var enteredPassword = AesHelper.Encrypt(request.Password);
            var token = await _postRepo.AuthenticationAsync(request);
            return token;
        }

        public async Task<bool> CreateAccountAsync(CreateAccountRequest request)
        {
            if (request is null)
                return false;
            var encryptedPass = AesHelper.Encrypt(request.Password);
            var newAcc = new Account()
            {
                AccountCode = request.AccountCode,
                Username = request.Username,
                EncryptedPassword = encryptedPass
            };
            var createAccount = await _postRepo.CreateAccountAsync(newAcc);
            if (!createAccount)
                return false;
            return createAccount;
        }

        public async Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            if (request is null)
                return false;

            int roleId = 0;
            switch(request.RolePosition)
            {
                case "Administrator":
                    roleId = 1;
                    break;
                case "Cashier":
                    roleId = 2;
                    break;
                case "Inventory Manager":
                    roleId = 3;
                    break;
            }

            var newEmployee = new Employee
            {
                EmployeeCode = request.EmployeeCode,
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                Age = request.Age,
                BirthDate = request.BirthDate,
                CivilStatus = request.CivilStatus,
                Gender = request.Gender,
                Email = request.Email,
                Address = request.Address,
                RoleId = roleId
            };

            var createEmployee = await _postRepo.CreateEmployeeAsync(newEmployee);
            if (!createEmployee)
                return false;
            return createEmployee;
        }
    }
}
