using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using System.Diagnostics.Contracts;

namespace PharmacyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetController : ControllerBase
    {
        private readonly IGetServiceContracts _getService;

        public GetController(IGetServiceContracts getService)
        {
            _getService = getService;
        }

        [HttpGet("EmployeeAll")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployeesAsync()
        {
            var employees = await _getService.GetAllEmployeesAsync();
            if (!employees.Any() || employees.Count() == 0)
                return NotFound("There is no employees currently registered.");
            return Ok(employees);
        }
        [HttpGet("Employee")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeByIdAsync(int id)
        {
            var employee = await _getService.GetEmployeeByIdAsync(id);
            if (employee is null)
                return NotFound("Employee does not exist.");
            return Ok(employee);
        }

        [HttpGet("SaleAll")]
        public async Task<ActionResult<IEnumerable<SalesTransactionDto>>> GetAllSalesAsync()
        {
            var sales = await _getService.GetAllSalesTransactionsAsync();
            if (!sales.Any())
                return NotFound("No sales transactions found.");
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesTransactionDto>> GetSaleByIdAsync(int id)
        {
            var sale = await _getService.GetSalesTransactionByIdAsync(id);
            if (sale == null)
                return NotFound("Sales transaction not found.");
            return Ok(sale);
        }
    }
}

