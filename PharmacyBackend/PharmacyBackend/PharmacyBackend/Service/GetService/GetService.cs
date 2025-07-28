using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;

namespace PharmacyBackend.Service.GetService
{
    public class GetService : IGetServiceContracts
    {
        private readonly IGetRepositoryContracts _getRepo;

        public GetService(IGetRepositoryContracts getRepo)
        {
            _getRepo = getRepo;
        }

        public async Task<AccountDTO> GetAccountByIdAsync(int id)
        {
            var account = await _getRepo.GetAccountByIdAsync(id);
            if (account is null)
                return null;
            var modifAccount = new AccountDTO
            {
                Id = account.Id,
                AccountCode = account.AccountCode,
                Username = account.Username,
                DecryptedPassword = account.EncryptedPassword,
            };
            return modifAccount;
        }

        public async Task<List<AccountDTO>> GetAllAccountsAsync()
        {
            var accounts = await _getRepo.GetAllAccountsAsync();
            if (!accounts.Any() && accounts.Count() == 0)
                return new List<AccountDTO>();
            var modifAccounts = accounts.Select(a => new AccountDTO
            {
                Id = a.Id,
                AccountCode = a.AccountCode,
                Username = a.Username,
                DecryptedPassword = a.EncryptedPassword,
            }).ToList();
            return modifAccounts;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _getRepo.GetAllEmployeesAsync();
            if (!employees.Any() || employees.Count() == 0)
                return new List<EmployeeDTO>();
            var modifEmployees = employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                LastName = e.LastName,
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                Age = e.Age,
                Gender = e.Gender,
                Email = e.Email,
                Address = e.Address
            }).ToList();
            return modifEmployees;
        }

        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _getRepo.GetEmployeeByIdAsync(id);
            if (employee is null)
                return null;
            var modifEmployee = new EmployeeDTO
            {
                EmployeeCode = employee.EmployeeCode,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                Age = employee.Age,
                Gender = employee.Gender,
                Email = employee.Email,
                Address = employee.Address
            };
            return modifEmployee;
        }
    }
}
