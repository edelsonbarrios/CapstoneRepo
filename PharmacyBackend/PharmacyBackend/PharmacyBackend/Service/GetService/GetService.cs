using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;

namespace PharmacyBackend.Service.GetService
{
    public class GetService : IGetServiceContracts
    {
        private readonly IGetRepositoryContracts _getRepo;

        public GetService(IGetRepositoryContracts getRepo)
        {
            _getRepo = getRepo;
        }

        //employee
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

        //sales

        public async Task<List<SalesTransactionDto>> GetAllSalesTransactionsAsync()
        {
            var transactions = await _getRepo.GetAllSalesTransactionsAsync();
            return transactions.Select(t => new SalesTransactionDto
            {
                Id = t.Id,
                TransactionDate = t.TransactionDate,
                InvoiceNumber = t.InvoiceNumber,
                TotalAmount = t.TotalAmount,
                DiscountAmount = t.DiscountAmount,
                NetAmount = t.NetAmount,
                Items = t.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            }).ToList();
        }

        public async Task<SalesTransactionDto?> GetSalesTransactionByIdAsync(int id)
        {
            var transaction = await _getRepo.GetSalesTransactionByIdAsync(id);
            if (transaction is null) return null;

            return new SalesTransactionDto
            {
                Id = transaction.Id,
                TransactionDate = transaction.TransactionDate,
                InvoiceNumber = transaction.InvoiceNumber,
                TotalAmount = transaction.TotalAmount,
                DiscountAmount = transaction.DiscountAmount,
                NetAmount = transaction.NetAmount,
                Items = transaction.Items.Select(i => new SalesItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }

        //Discount
        public async Task<IEnumerable<DiscountDto>> GetAllDiscountAsync()
        {
            var discounts = await _getRepo.GetAllDiscountAsync();
            return discounts.Select(d => new DiscountDto
            {
                Id = d.Id,
                DiscountName = d.DiscountName,
                DiscountRate = d.DiscountRate
            });
        }

        public async Task<DiscountDto?> GetDiscountByIdAsync(int id)
        {
            var d = await _getRepo.GetDiscountByIdAsync(id);
            if (d == null) return null;

            return new DiscountDto
            {
                Id = d.Id,
                DiscountName = d.DiscountName,
                DiscountRate = d.DiscountRate
            };
        }
    }
}

