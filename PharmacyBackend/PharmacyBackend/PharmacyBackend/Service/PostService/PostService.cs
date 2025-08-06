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

        public Task<bool> CreateAccountAsync(AccountDTO request)
        {
            
        }

        public Task<bool> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            throw new NotImplementedException();
        }

        public string TokenGeneration(AccountDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
