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

        public async Task<bool> AuthenticationAsync(LogInRequest request)
        {
            var enteredPassword = AesHelper.Encrypt(request.Password);
            var logIn = await _postRepo.AuthenticationAsync(request);
            if (!logIn)
                return false;
            return true;
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

        public Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
