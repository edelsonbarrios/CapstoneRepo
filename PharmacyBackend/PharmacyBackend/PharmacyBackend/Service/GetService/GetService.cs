using PharmacyBackend.Contracts.RepositoryContracts.GetInterface;
using PharmacyBackend.Contracts.ServiceContracts.GetInterface;
using PharmacyBackend.Models;
using PharmacyBackend.DTOs.EmployeeDTOs;
using PharmacyBackend.DTOs.AccountDTOs;
using PharmacyBackend.DTOs.SalesDTOs;
using PharmacyBackend.DTOs.DiscountDTOs;
using PharmacyBackend.DTOs.ProductDTOs;
using PharmacyBackend.DTOs.CategoryDTOs;
using PharmacyBackend.DTOs.ItemDataControlDTOs;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PharmacyBackend.DTOs.InventoryDTOs;
using PharmacyBackend.DTOs.PuchaseOrderDTOs;
using PharmacyBackend.DTOs.PurchaseOrderItemDTOs;
using PharmacyBackend.DTOs.StockMovementDTOs;
using PharmacyBackend.DTOs.SupplierDTOs;
using PharmacyBackend.DTOs.VatEntryDTOs;
using PharmacyBackend.DTOs.EmployeeHistoryDTO;
using PharmacyBackend.DTOs.DashboardDTOs;
using PharmacyBackend.Contracts.RepositoryContracts.UpdateInterface;
using PharmacyBackend.DTOs.DailySaleDTOs;
using PharmacyBackend.DTOs.ReportsDTOs;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using PharmacyBackend.DTOs.DataBaseBackUpDTOs;
using PharmacyBackend.Repository.GET;
using Microsoft.EntityFrameworkCore.Update;
using PharmacyBackend.DTOs.StockHistoryDTOs;
using System.Runtime.Intrinsics.Arm;
using PharmacyBackend.Helper;

namespace PharmacyBackend.Service.GetService
{
  public class GetService : IGetServiceContracts
  {
    private readonly IGetRepositoryContracts _getRepo;
    private readonly IUpdateRepositoryContracts _updateRepo;
    private readonly IConfiguration _config;
    private readonly ILogger<GetService> _logger;

    public GetService(IGetRepositoryContracts getRepo, 
        IUpdateRepositoryContracts updateRepo,
        IConfiguration config,
        ILogger<GetService> logger)
    {
      _getRepo = getRepo;
      _updateRepo = updateRepo;
      _config = config;
      _logger = logger;
    }

    #region DashBoardInventoryMan

    public async Task<InventoryDashboardDto> GetInventoryDashboardAsync()
    {
      var dashboard = new InventoryDashboardDto();

      var allProducts = await GetAllProductAsync();
      var allInventory = await GetAllInventoryAsync();
      var criticalStock = await GetCriticalStockInventoryAsync();
      var nearExpiry = await GetNearExpiryInventoryAsync();
      var expiredStock = await GetExpiredInventoryAsync();

      // Calculate summary metrics
      dashboard.TotalActiveProducts = allProducts.Count(p => p.IsActive);
      dashboard.TotalInventoryItems = allInventory.Count(i => i.IsActive);
      dashboard.CriticalStockCount = criticalStock.Count();
      dashboard.NearExpiryCount = nearExpiry.Count();
      dashboard.ExpiredStockCount = expiredStock.Count(i => i.IsDeleted == false);

      // Populate detailed lists (limited to first 10 for performance)
      dashboard.CriticalStockItems = criticalStock.ToList();
      dashboard.NearExpiryItems = nearExpiry.ToList();
      dashboard.ExpiredStockItems = expiredStock.ToList(); 

      return dashboard;
    }
    #endregion

    #region Employees
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
          BirthDate = e.BirthDate,
          CivilStatus = e.CivilStatus,
          Gender = e.Gender,
          Email = e.Email,
          PhoneNumber = e.PhoneNumber,
          Address = e.Address,
          FullName = e.FullName,
          RolePosition = e.Role.RolePosition,
          IsSuperAdmin = e.IsSuperAdmin,
          Account = new AccountDTO
          {
          Id = e.Account.Id,
          EmployeeId = e.Account.EmployeeId,
          Username = e.Account.Username,
          Password = e.Account.EncryptedPassword
          }
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
        Id = employee.Id,
        EmployeeCode = employee.EmployeeCode,
        LastName = employee.LastName,
        FirstName = employee.FirstName,
        MiddleName = employee.MiddleName,
        Age = employee.Age,
        BirthDate = employee.BirthDate,
        CivilStatus = employee.CivilStatus,
        Gender = employee.Gender,
        Email = employee.Email,
        PhoneNumber = employee.PhoneNumber,
        Address = employee.Address,
        FullName = employee.FullName,
        RolePosition = employee.Role.RolePosition,
        Account = new AccountDTO
        {
          Id = employee.Account.Id,
          EmployeeId = employee.Account.EmployeeId,
          Username = employee.Account.Username,
          Password = employee.Account.EncryptedPassword
        }
      };
      return modifEmployee;
    }

    public async Task<List<EmployeeDTO>> GetEmployeeByLastName(string lastName)
    {
      if (string.IsNullOrEmpty(lastName))
        return null;
      var employee = await _getRepo.GetEmployeeByLastName(lastName);
      if (employee is null)
        return null;
      var modifEmployees = employee.Select(e => new EmployeeDTO
          {
          Id = e.Id,
          EmployeeCode = e.EmployeeCode,
          LastName = e.LastName,
          FirstName = e.FirstName,
          MiddleName = e.MiddleName,
          Age = e.Age,
          BirthDate = e.BirthDate,
          CivilStatus = e.CivilStatus,
          Gender = e.Gender,
          Email = e.Email,
          PhoneNumber = e.PhoneNumber,
          Address = e.Address,
          RolePosition = e.Role.RolePosition,
          FullName = e.FullName
          }).ToList();
      return modifEmployees;
    }

    public async Task<List<EmployeeDTO>> GetFirstDisplayEmployees()
    {
      var employees = await _getRepo.GetFirstDisplayEmployees();
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
          BirthDate = e.BirthDate,
          CivilStatus = e.CivilStatus,
          Gender = e.Gender,
          Email = e.Email,
          PhoneNumber = e.PhoneNumber,
          Address = e.Address,
          RolePosition = e.Role.RolePosition,
          FullName = e.FullName
          }).ToList();
      return modifEmployees;
    }
    #endregion

    #region EmployeeHistory
    public async Task<List<EmployeeHistoryDTO>> GetAllEmployeeHistoryAsync()
    {
      var employeeHistories = await _getRepo.GetAllEmployeeHistoryAsync();
      if (!employeeHistories.Any() || employeeHistories.Count() == 0)
        return new List<EmployeeHistoryDTO>();
      var modifHistories = employeeHistories.Select(e => new EmployeeHistoryDTO
          {
          EmployeeChanged = e.Employee.FullName,
          ActionPerformed = e.ActionPerformed,
          PerformedBy = e.PerformedByEmployee.FullName,
          PerformedAt = e.PerformedAt
          }).ToList();
      return modifHistories;
    }
    public async Task<EmployeeHistoryDTO> GetEmployeeHistoryByIdAsync(int employeeHistoryId)
    {
      var employeeHistory = await _getRepo.GetEmployeeHistoryByIdAsync(employeeHistoryId);
      if (employeeHistory is null)
        return null;
      var modifHistory = new EmployeeHistoryDTO()
      {
        EmployeeChanged = employeeHistory.Employee.FullName,
        ActionPerformed = employeeHistory.ActionPerformed,
        PerformedBy = employeeHistory.PerformedByEmployee.FullName,
        PerformedAt = employeeHistory.PerformedAt
      };
      return modifHistory;
    }
    public async Task<List<EmployeeHistoryDTO>> GetDisplayEmployeeHistoryAsync()
    {
      var employeeHistories = await _getRepo.GetDisplayEmployeeHistoryAsync();
      if (!employeeHistories.Any() || employeeHistories.Count() == 0)
        return new List<EmployeeHistoryDTO>();

      var modifHistories = employeeHistories.Select(e => new EmployeeHistoryDTO
          {
          EmployeeChanged = e.Employee.FullName,
          ActionPerformed = e.ActionPerformed,
          PerformedBy = e.PerformedByEmployee.FullName,
          PerformedAt = e.PerformedAt
          }).ToList();

      return modifHistories;
    }
    #endregion

    #region Account
    public async Task<bool> AccountUsernameValidation(string userName)
    {
      if (string.IsNullOrEmpty(userName))
        return false;
      var validateUsername = await _getRepo.AccountUsernameValidation(userName);
      return validateUsername;
    }

    public async Task<List<AccountDTO>> GetAllAccountsAsync()
    {
      var accounts = await _getRepo.GetAllAccountsAsync();
      if (!accounts.Any() && accounts.Count() == 0)
        return new List<AccountDTO>();
      var modifAccounts = accounts.Select(a => new AccountDTO
          {
          Id = a.Id,
          Username = a.Username,
          Password = a.EncryptedPassword,
          EmployeeId = a.EmployeeId
          }).ToList();
      return modifAccounts;
    }

    public async Task<AccountDTO> GetAccountByIdAsync(int id)
    {
      var account = await _getRepo.GetAccountByIdAsync(id);
      if (account is null)
        return null;
      var modifAccount = new AccountDTO
      {
        Id = account.Id,
        Username = account.Username,
        Password = account.EncryptedPassword,
        EmployeeId = account.EmployeeId
      };
      return modifAccount;
    }

    public async Task<AccountDTO> SearchAccountByUsernameAsync(string userName)
    {
      var account = await _getRepo.SearchAccountByUsernameAsync(userName);
      if (account is null)
        return null;
      var modifAccount = new AccountDTO
      {
        Id = account.Id,
        Username = account.Username,
        Password = account.EncryptedPassword,
        EmployeeId = account.EmployeeId
      };
      return modifAccount;
    }

    public async Task<AccountDTO> GetAccountByEmployeeIdAsync(int employeeId)
    {
      if (employeeId == 0)
        return new AccountDTO();
      var account = await _getRepo.GetAccountByEmployeeIdAsync(employeeId);
      if (account is null)
        return new AccountDTO();
      var modifEmployee = new AccountDTO
      {
        Id = account.Id,
        Username = account.Username,
        Password = AesHelper.Decrypt(account.EncryptedPassword),
        EmployeeId = account.EmployeeId
      };
      return modifEmployee;
    }

    #endregion

    #region Daily Sales
    public async Task<DailySalesSummaryDto> GetSummarySalesForToday()
    {
      var transaction = await _getRepo.GetSummarySalesForToday();
      if (transaction is null)
        return new DailySalesSummaryDto();
      return transaction;
    }

    public async Task<DailySalesSummaryDto> GetDailySalesSummaryAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? employeeId = null,
        string paymentMethod = null)
    {
      var summary = await _getRepo.GetDailySalesSummaryAsync(
          startDate,
          endDate,
          employeeId,
          paymentMethod
          );

      return summary;
    }

    public async Task<IEnumerable<ProductSalesSummaryDto>> GetProductSalesSummaryAsync(
        DateTime startDate,
        DateTime endDate)
    {
      var products = await _getRepo.GetProductSalesSummaryAsync(startDate, endDate);

      return products;
    }

    public async Task<IEnumerable<SalesTransactionDto>> GetFilteredSalesTransactionsAsync(
        DateTime startDate,
        DateTime endDate,
        int? employeeId = null,
        string paymentMethod = null)
    {
      var transactions = await _getRepo.GetFilteredSalesTransactionsAsync(
          startDate,
          endDate,
          employeeId,
          paymentMethod
          );

      // Map to DTO
      var transactionDtos = transactions.Select(t => new SalesTransactionDto
          {
          Id = t.Id,
          TransactionDate = t.TransactionDate,
          InvoiceNumber = t.InvoiceNumber,
          TotalAmount = t.TotalAmount,
          DiscountAmount = t.DiscountAmount,
          VATAmount = t.VATAmount,
          NetAmount = t.NetAmount,
          PaymentMethod = t.PaymentMethod,
          AmountPaid = t.AmountPaid,
          ChangeAmount = t.ChangeAmount,
          DiscountId = t.DiscountId,
          EmployeeID = t.EmployeeID,
          VATId = t.VATId,
          DiscountName = t.Discount?.DiscountName ?? string.Empty,
          EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
          VATRate = t.VAT?.VATRate,
          Items = t.Items.Select(i => new SalesItemDto
              {
              Id = i.Id,
              SalesTransactionId = i.SalesTransactionId,
              ProductId = i.ProductId,
              ProductName = i.Product?.ProductName ?? string.Empty,
              UnitType = i.UnitType,
              Quantity = i.Quantity,
              UnitPrice = i.UnitPrice,
              Subtotal = i.Subtotal
              }).ToList()
          });

      return transactionDtos;
    }

    #endregion

    #region Sales

    public async Task<SalesTransactionDto?> GetSalesTransactionByIdAsync(int id)
    {
      var transaction = await _getRepo.GetSalesTransactionByIdAsync(id);
      if (transaction == null)
        return null;

      return new SalesTransactionDto
      {
        Id = transaction.Id,
        TransactionDate = transaction.TransactionDate,
        InvoiceNumber = transaction.InvoiceNumber,
        TotalAmount = transaction.TotalAmount,
        DiscountAmount = transaction.DiscountAmount,
        VATAmount = transaction.VATAmount,
        NetAmount = transaction.NetAmount,
        PaymentMethod = transaction.PaymentMethod,
        AmountPaid = transaction.AmountPaid,
        ChangeAmount = transaction.ChangeAmount,
        DiscountId = transaction.DiscountId,
        EmployeeID = transaction.EmployeeID,
        VATId = transaction.VATId,
        DiscountName = transaction.Discount?.DiscountName ?? string.Empty,
        EmployeeName = transaction.Employee != null ? $"{transaction.Employee.FirstName} {transaction.Employee.LastName}" : string.Empty,
        VATRate = transaction.VAT?.VATRate,
        Items = transaction.Items.Select(i => new SalesItemDto
            {
            Id = i.Id,
            SalesTransactionId = i.SalesTransactionId,
            ProductId = i.ProductId,
            ProductName = i.Product?.ProductName ?? string.Empty,
            UnitType = i.UnitType,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            Subtotal = i.Subtotal
            }).ToList()
      };
    }

    public async Task<IEnumerable<SalesTransactionDto>> GetAllSalesTransactionsAsync()
    {
      var transactions = await _getRepo.GetAllSalesTransactionsAsync();
      return transactions.Select(t => new SalesTransactionDto
          {
          Id = t.Id,
          TransactionDate = t.TransactionDate,
          InvoiceNumber = t.InvoiceNumber,
          TotalAmount = t.TotalAmount,
          DiscountAmount = t.DiscountAmount,
          VATAmount = t.VATAmount,
          NetAmount = t.NetAmount,
          PaymentMethod = t.PaymentMethod,
          ReferenceNumber = t.ReferenceNumber,
          AmountPaid = t.AmountPaid,
          ChangeAmount = t.ChangeAmount,
          DiscountId = t.DiscountId,
          EmployeeID = t.EmployeeID,
          VATId = t.VATId,
          DiscountName = t.Discount?.DiscountName ?? string.Empty,
          EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
          VATRate = t.VAT?.VATRate,
          Items = t.Items.Select(i => new SalesItemDto
              {
              Id = i.Id,
              ProductId = i.ProductId,
              ProductName = i.Product?.ProductName ?? string.Empty,
              UnitType = i.UnitType,
              Quantity = i.Quantity,
              UnitPrice = i.UnitPrice,
              Subtotal = i.Subtotal
              }).ToList()
          });
    }

    public async Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByEmployeeIdAsync(int employeeId)
    {
      var transactions = await _getRepo.GetSalesTransactionsByEmployeeIdAsync(employeeId);
      return transactions.Select(t => new SalesTransactionDto
          {
          Id = t.Id,
          TransactionDate = t.TransactionDate,
          InvoiceNumber = t.InvoiceNumber,
          TotalAmount = t.TotalAmount,
          DiscountAmount = t.DiscountAmount,
          VATAmount = t.VATAmount,
          NetAmount = t.NetAmount,
          PaymentMethod = t.PaymentMethod,
          AmountPaid = t.AmountPaid,
          ChangeAmount = t.ChangeAmount,
          DiscountId = t.DiscountId,
          EmployeeID = t.EmployeeID,
          VATId = t.VATId,
          DiscountName = t.Discount?.DiscountName ?? string.Empty,
          EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
          VATRate = t.VAT?.VATRate,
          Items = t.Items.Select(i => new SalesItemDto
              {
              Id = i.Id,
              ProductId = i.ProductId,
              ProductName = i.Product?.ProductName ?? string.Empty,
              UnitType = i.UnitType,
              Quantity = i.Quantity,
              UnitPrice = i.UnitPrice,
              Subtotal = i.Subtotal
              }).ToList()
          });
    }

    public async Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
      var transactions = await _getRepo.GetSalesTransactionsByDateRangeAsync(startDate, endDate);
      return transactions.Select(t => new SalesTransactionDto
          {
          Id = t.Id,
          TransactionDate = t.TransactionDate,
          InvoiceNumber = t.InvoiceNumber,
          TotalAmount = t.TotalAmount,
          DiscountAmount = t.DiscountAmount,
          VATAmount = t.VATAmount,
          NetAmount = t.NetAmount,
          PaymentMethod = t.PaymentMethod,
          AmountPaid = t.AmountPaid,
          ChangeAmount = t.ChangeAmount,
          DiscountId = t.DiscountId,
          EmployeeID = t.EmployeeID,
          VATId = t.VATId,
          DiscountName = t.Discount?.DiscountName ?? string.Empty,
          EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
          VATRate = t.VAT?.VATRate,
          Items = t.Items.Select(i => new SalesItemDto
              {
              Id = i.Id,
              ProductId = i.ProductId,
              ProductName = i.Product?.ProductName ?? string.Empty,
              UnitType = i.UnitType,
              Quantity = i.Quantity,
              UnitPrice = i.UnitPrice,
              Subtotal = i.Subtotal
              }).ToList()
          });
    }

    public async Task<IEnumerable<SalesTransactionDto>> GetSalesTransactionsByInvoiceNumberAsync(string invoiceNumber)
    {
      var transactions = await _getRepo.GetSalesTransactionsByInvoiceNumberAsync(invoiceNumber);
      return transactions.Select(t => new SalesTransactionDto
          {
          Id = t.Id,
          TransactionDate = t.TransactionDate,
          InvoiceNumber = t.InvoiceNumber,
          TotalAmount = t.TotalAmount,
          DiscountAmount = t.DiscountAmount,
          VATAmount = t.VATAmount,
          NetAmount = t.NetAmount,
          PaymentMethod = t.PaymentMethod,
          AmountPaid = t.AmountPaid,
          ChangeAmount = t.ChangeAmount,
          DiscountId = t.DiscountId,
          EmployeeID = t.EmployeeID,
          VATId = t.VATId,
          DiscountName = t.Discount?.DiscountName ?? string.Empty,
          EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
          VATRate = t.VAT?.VATRate,
          Items = t.Items.Select(i => new SalesItemDto
              {
              Id = i.Id,
              ProductId = i.ProductId,
              ProductName = i.Product?.ProductName ?? string.Empty,
              UnitType = i.UnitType,
              Quantity = i.Quantity,
              UnitPrice = i.UnitPrice,
              Subtotal = i.Subtotal
              }).ToList()
          });
    }

    public async Task<SalesItemDto?> GetSalesItemByIdAsync(int id)
    {
      var item = await _getRepo.GetSalesItemByIdAsync(id);
      if (item == null)
        return null;

      return new SalesItemDto
      {
        Id = item.Id,
        ProductId = item.ProductId,
        ProductName = item.Product?.ProductName ?? string.Empty,
        UnitType = item.UnitType,
        Quantity = item.Quantity,
        UnitPrice = item.UnitPrice,
        Subtotal = item.Subtotal
      };
    }

    public async Task<IEnumerable<SalesItemDto>> GetSalesItemsByTransactionIdAsync(int transactionId)
    {
      var items = await _getRepo.GetSalesItemsByTransactionIdAsync(transactionId);
      return items.Select(i => new SalesItemDto
          {
          Id = i.Id,
          ProductId = i.ProductId,
          ProductName = i.Product?.ProductName ?? string.Empty,
          UnitType = i.UnitType,
          Quantity = i.Quantity,
          UnitPrice = i.UnitPrice,
          Subtotal = i.Subtotal
          });
    }


    public async Task<bool> ReferenceNumberValidationAsync(string refNum) 
    {
      var refNumValidation = await _getRepo.ReferenceNumberValidationAsync(refNum);
      if(refNumValidation)
        return false;
       return true;
    }
    
    #endregion

    #region Discount
    public async Task<IEnumerable<DiscountDto>> GetAllDiscountAsync()
    {
      var discounts = await _getRepo.GetAllDiscountAsync();
      return discounts.Select(d => new DiscountDto
          {
          Id = d.Id,
          DiscountName = d.DiscountName,
          DiscountType = d.DiscountType,
          DiscountAmount = d.DiscountAmount,
          StartDate = d.StartDate,
          EndDate = d.EndDate,
          IsActive = d.IsActive
          });
    }

    public async Task<DiscountDto?> GetDiscountByIdAsync(int id)
    {
      var d = await _getRepo.GetDiscountByIdAsync(id);
      if (d == null)
        return null;
      return new DiscountDto
      {
        Id = d.Id,
        DiscountName = d.DiscountName,
        DiscountType = d.DiscountType,
        DiscountAmount = d.DiscountAmount,
        StartDate = d.StartDate,
        EndDate = d.EndDate,
        IsActive = d.IsActive
      };
    }

    public async Task<List<DiscountDto>> GetAllAvailableDiscountsAsync() 
    {
      var discounts = await _getRepo.GetAllAvailableDiscountsAsync();
      
      if(!discounts.Any() || discounts.Count() == 0)
        return new List<DiscountDto>();

      return discounts.Select(d => new DiscountDto {
          Id = d.Id,
          DiscountName = d.DiscountName,
          DiscountType = d.DiscountType,
          DiscountAmount = d.DiscountAmount,
          StartDate = d.StartDate,
          EndDate = d.EndDate,
          IsActive = d.IsActive,
          }).ToList();
    }

    #endregion region

    #region Products
    public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
    {
      var products = await _getRepo.GetAllProductAsync();
      return products.Select(p => new ProductDto
          {
          ProductID = p.ProductID,
          ProductName = p.ProductName,
          CategoryID = p.CategoryID,
          BrandID = p.BrandID,
          GenericID = p.GenericID,
          FormulationID = p.FormulationID,
          ClassificationID = p.ClassificationID,
          TypeID = p.TypeID,
          IsActive = p.IsActive,
          DateCreated = p.DateCreated,
          SheetsPerBox = p.SheetsPerBox,
          PiecesPerSheet = p.PiecesPerSheet,
          BoxPrice = p.BoxPrice,
          SheetPrice = p.SheetPrice,
          PiecePrice = p.PiecePrice,
          ReorderLevel = p.ReorderLevel,
          UnitMeasurement = p.UnitMeasurement,
          Description = p.Description,
          PiecePerBox = p.PiecePerBox,
          CostPrice = p.CostPrice,
          CostBoxPrice = p.CostBoxPrice,
          CostSheetPrice = p.CostSheetPrice,
          CostPiecePrice = p.CostPiecePrice,



          // Inventory calculations
          //TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
          ExpiredStock = p.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
          NearExpiryStock = p.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
          IsCriticalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) <= p.Inventory?.FirstOrDefault()?.ReorderLevel,
          HasExpiredStock = p.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

          // Related entity names
          CategoryName = p.Category?.CategoryName ?? string.Empty,
          BrandName = p.Brand?.BrandName ?? string.Empty,
          GenericName = p.Generic?.GenericName ?? string.Empty,
          FormulationName = p.Formulation?.FormulationName ?? string.Empty,
          ClassificationName = p.Classification?.ClassificationName ?? string.Empty,
          TypeName = p.TypeModel?.TypeName ?? string.Empty,
          });
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
      var p = await _getRepo.GetProductByIdAsync(id);
      if (p == null)
        return null;

      return new ProductDto
      {
        ProductID = p.ProductID,
        ProductName = p.ProductName,
        CategoryID = p.CategoryID,
        BrandID = p.BrandID,
        GenericID = p.GenericID,
        FormulationID = p.FormulationID,
        ClassificationID = p.ClassificationID,
        TypeID = p.TypeID,
        IsActive = p.IsActive,
        DateCreated = p.DateCreated,
        SheetsPerBox = p.SheetsPerBox,
        PiecesPerSheet = p.PiecesPerSheet,
        BoxPrice = p.BoxPrice,
        SheetPrice = p.SheetPrice,
        PiecePrice = p.PiecePrice,
        ReorderLevel = p.ReorderLevel,
        UnitMeasurement = p.UnitMeasurement,
        Description = p.Description,
        PiecePerBox = p.PiecePerBox,

        // Inventory calculations
        //TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
        ExpiredStock = p.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
        NearExpiryStock = p.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
        IsCriticalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) <= p.Inventory?.FirstOrDefault()?.ReorderLevel,
        HasExpiredStock = p.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

        // Related entity names
        CategoryName = p.Category?.CategoryName ?? string.Empty,
        BrandName = p.Brand?.BrandName ?? string.Empty,
        GenericName = p.Generic?.GenericName ?? string.Empty,
        FormulationName = p.Formulation?.FormulationName ?? string.Empty,
        ClassificationName = p.Classification?.ClassificationName ?? string.Empty,
        TypeName = p.TypeModel?.TypeName ?? string.Empty,
      };
    }
    public async Task<List<ProductDto>> GetAllInactiveProductsAsync() 
    {
      var inactiveProducts = await _getRepo.GetAllInactiveProductsAsync();
      if(!inactiveProducts.Any() || inactiveProducts.Count() == 0)
        return new List<ProductDto>();
      var modifInactiveProducts = inactiveProducts.Select(p => new ProductDto {
          ProductID = p.ProductID,
          ProductName = p.ProductName,
          CategoryID = p.CategoryID,
          BrandID = p.BrandID,
          GenericID = p.GenericID,
          FormulationID = p.FormulationID,
          ClassificationID = p.ClassificationID,
          TypeID = p.TypeID,
          IsActive = p.IsActive,
          DateCreated = p.DateCreated,
          SheetsPerBox = p.SheetsPerBox,
          PiecesPerSheet = p.PiecesPerSheet,
          BoxPrice = p.BoxPrice,
          SheetPrice = p.SheetPrice,
          PiecePrice = p.PiecePrice,
          ReorderLevel = p.ReorderLevel,
          UnitMeasurement = p.UnitMeasurement,
          Description = p.Description,
          PiecePerBox = p.PiecePerBox,

          // Inventory calculations
          //TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
          ExpiredStock = p.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
          NearExpiryStock = p.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
          IsCriticalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) <= p.Inventory?.FirstOrDefault()?.ReorderLevel,
          HasExpiredStock = p.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

          // Related entity names
          CategoryName = p.Category?.CategoryName ?? string.Empty,
          BrandName = p.Brand?.BrandName ?? string.Empty,
          GenericName = p.Generic?.GenericName ?? string.Empty,
          FormulationName = p.Formulation?.FormulationName ?? string.Empty,
          ClassificationName = p.Classification?.ClassificationName ?? string.Empty,
          TypeName = p.TypeModel?.TypeName ?? string.Empty,
      }).ToList();
      return modifInactiveProducts;
    }

    public async Task<ProductDto> GetProductByNameAsync(string name) 
    {
      var product = await _getRepo.GetProductByNameAsync(name);
      if(product is null)
        return new ProductDto();
      return new ProductDto
      {
        ProductID = product.ProductID,
        ProductName = product.ProductName,
        CategoryID = product.CategoryID,
        BrandID = product.BrandID,
        GenericID = product.GenericID,
        FormulationID = product.FormulationID,
        ClassificationID = product.ClassificationID,
        TypeID = product.TypeID,
        IsActive = product.IsActive,
        DateCreated = product.DateCreated,
        SheetsPerBox = product.SheetsPerBox,
        PiecesPerSheet = product.PiecesPerSheet,
        BoxPrice = product.BoxPrice,
        SheetPrice = product.SheetPrice,
        PiecePrice = product.PiecePrice,
        ReorderLevel = product.ReorderLevel,
        UnitMeasurement = product.UnitMeasurement,
        Description = product.Description,
        PiecePerBox = product.PiecePerBox,

        // Inventory calculations
        //TotalStock = p.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
        ExpiredStock = product.Inventory?.Where(i => i.IsActive && i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
        NearExpiryStock = product.Inventory?.Where(i => i.IsActive && i.IsNearExpiry && !i.IsExpired).Sum(i => i.BoxQuantity) ?? 0,
        IsCriticalStock = product.Inventory?.Where(i => i.IsActive && !i.IsExpired).Sum(i => i.BoxQuantity) <= product.Inventory?.FirstOrDefault()?.ReorderLevel,
        HasExpiredStock = product.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

        // Related entity names
        CategoryName = product.Category?.CategoryName ?? string.Empty,
        BrandName = product.Brand?.BrandName ?? string.Empty,
        GenericName = product.Generic?.GenericName ?? string.Empty,
        FormulationName = product.Formulation?.FormulationName ?? string.Empty,
        ClassificationName = product.Classification?.ClassificationName ?? string.Empty,
        TypeName = product.TypeModel?.TypeName ?? string.Empty,
      };

    }
    #endregion

    #region Category
    public async Task<IEnumerable<CategoryDto>> GetAllCategoryAsync()
    {
      var categories = await _getRepo.GetAllCategoryAsync();
      return categories.Select(c => new CategoryDto
          {
          CategoryID = c.CategoryID,
          CategoryName = c.CategoryName
          });
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
      var c = await _getRepo.GetCategoryByIdAsync(id);
      if (c == null)
        return null;
      return new CategoryDto
      {
        CategoryID = c.CategoryID,
        CategoryName = c.CategoryName
      };
    }

    public async Task<List<CategoryDto>> GetAllInactiveCategoriesAsync() 
    {
      var inactiveCategories = await _getRepo.GetAllInactiveCategoriesAsync();
      if(!inactiveCategories.Any() || inactiveCategories.Count() == 0)
        return new List<CategoryDto>();
      return inactiveCategories.Select(c => new CategoryDto {
          CategoryID = c.CategoryID,
          CategoryName = c.CategoryName
          }).ToList();
    }
    #endregion

    #region Brand
    public async Task<IEnumerable<BrandDto>> GetAllBrandAsync()
    {
      var brand = await _getRepo.GetAllBrandAsync();
      return brand.Select(b => new BrandDto
          {
          BrandID = b.BrandID,
          BrandName = b.BrandName
          });
    }

    public async Task<BrandDto?> GetBrandByIdAsync(int id)
    {
      var b = await _getRepo.GetBrandByIdAsync(id);
      if (b == null)
        return null;
      return new BrandDto
      {
        BrandID = b.BrandID,
        BrandName = b.BrandName
      };
    }
    public async Task<List<BrandDto>> GetAllInactiveBrandsAsync() 
    {
      var inactiveBrands = await _getRepo.GetAllInactiveBrandsAsync();
      if(!inactiveBrands.Any() || inactiveBrands.Count() == 0)
        return new List<BrandDto>();
      return inactiveBrands.Select(b => new BrandDto {
          BrandID = b.BrandID,
          BrandName = b.BrandName,
          }).ToList();
    }
    #endregion

    #region Generic
    public async Task<IEnumerable<GenericDto>> GetAllGenericAsync()
    {
      var generic = await _getRepo.GetAllGenericAsync();
      return generic.Select(g => new GenericDto
          {
          GenericID = g.GenericID,
          GenericName = g.GenericName
          });
    }

    public async Task<GenericDto?> GetGenericByIdAsync(int id)
    {
      var g = await _getRepo.GetGenericByIdAsync(id);
      if (g == null)
        return null;
      return new GenericDto
      {
        GenericID = g.GenericID,
        GenericName = g.GenericName
      };
    }
    public async Task<List<GenericDto>> GetAllInactiveGenericsAsync() 
    {
      var inactiveGenerics = await _getRepo.GetAllInactiveGenericsAsync();
      return inactiveGenerics.Select(g => new GenericDto {
          GenericID = g.GenericID,
          GenericName = g.GenericName,
          }).ToList();
    }
    #endregion

    #region Formulation
    public async Task<IEnumerable<FormulationDto>> GetAllFormulationAsync()
    {
      var Formulation = await _getRepo.GetAllFormulationAsync();
      return Formulation.Select(f => new FormulationDto
          {
          FormulationID = f.FormulationID,
          FormulationName = f.FormulationName
          });
    }

    public async Task<FormulationDto?> GetFormulationByIdAsync(int id)
    {
      var f = await _getRepo.GetFormulationByIdAsync(id);
      if (f == null)
        return null;
      return new FormulationDto
      {
        FormulationID = f.FormulationID,
        FormulationName = f.FormulationName
      };
    }
    public async Task<List<FormulationDto>> GetAllInactiveFormulationsAsync() 
    {
      var inactiveFormulations = await _getRepo.GetAllInactiveFormulationsAsync();

      return inactiveFormulations.Select(f => new FormulationDto {
          FormulationID = f.FormulationID,
          FormulationName = f.FormulationName,
          }).ToList();
    }
    #endregion

    #region Classification
    public async Task<IEnumerable<ClassificationDto>> GetAllClassificationAsync()
    {
      var classification = await _getRepo.GetAllClassificationAsync();
      return classification.Select(c => new ClassificationDto
          {
          ClassificationID = c.ClassificationID,
          ClassificationName = c.ClassificationName
          });
    }

    public async Task<ClassificationDto?> GetClassificationByIdAsync(int id)
    {
      var c = await _getRepo.GetClassificationByIdAsync(id);
      if (c == null)
        return null;
      return new ClassificationDto
      {
        ClassificationID = c.ClassificationID,
        ClassificationName = c.ClassificationName
      };
    }

    public async Task<List<ClassificationDto>> GetAllInactiveClassificationsAsync() 
    {
      var inactiveClassifications = await _getRepo.GetAllInactiveClassificationsAsync();

      return inactiveClassifications.Select(c => new ClassificationDto {
          ClassificationID = c.ClassificationID,
          ClassificationName = c.ClassificationName,
          }).ToList();
    }

    #endregion

    #region Type
    public async Task<IEnumerable<TypeModelDto>> GetAllTypeModelAsync()
    {
      var type = await _getRepo.GetAllTypeModelAsync();
      return type.Select(t => new TypeModelDto
          {
          TypeID = t.TypeID,
          TypeName = t.TypeName
          });
    }

    public async Task<TypeModelDto?> GetTypeModelByIdAsync(int id)
    {
      var t = await _getRepo.GetTypeModelByIdAsync(id);
      if (t == null)
        return null;
      return new TypeModelDto
      {
        TypeID = t.TypeID,
        TypeName = t.TypeName
      };
    }

    public async Task<List<TypeModelDto>> GetAllInactiveTypesAsync() 
    {
      var inactiveTypes = await _getRepo.GetAllInactiveTypesAsync();

      return inactiveTypes.Select(t => new TypeModelDto {
          TypeID = t.TypeID,
          TypeName = t.TypeName,
          }).ToList();
    }

    #endregion

    #region Supplier
    public async Task<IEnumerable<SupplierDTO>> GetAllSupplierAsync()
    {
      var suppliers = await _getRepo.GetAllSupplierAsync();
      return suppliers.Select(s => new SupplierDTO
          {
          Id = s.Id,
          SupplierName = s.SupplierName,
          ContactNumber = s.ContactNumber,
          ContactPerson = s.ContactPerson,
          ContactPersonNumber = s.ContactPersonNumber,
          Email = s.Email,
          Address = s.Address,
          IsActive = s.IsActive,
          Products = s.Products.Select(p => new ProductDto
              {
              ProductID = p.ProductID,
              ProductName = p.ProductName,
              CategoryID = p.CategoryID,
              BrandID = p.BrandID,
              GenericID = p.GenericID,
              FormulationID = p.FormulationID,
              ClassificationID = p.ClassificationID,
              TypeID = p.TypeID,
              IsActive = p.IsActive,
              DateCreated = p.DateCreated,
              SheetsPerBox = p.SheetsPerBox,
              PiecesPerSheet = p.PiecesPerSheet,
              BoxPrice = p.BoxPrice,
              SheetPrice = p.SheetPrice,
              PiecePrice = p.PiecePrice,
              ReorderLevel = p.ReorderLevel,
              UnitMeasurement = p.UnitMeasurement,
              PiecePerBox = p.PiecePerBox,
              CostBoxPrice = p.CostBoxPrice,
              CostSheetPrice = p.CostSheetPrice,
              CostPiecePrice = p.CostPiecePrice,

              // Inventory calculations
              HasExpiredStock = p.Inventory?.Any(i => i.IsActive && i.IsExpired) ?? false,

              // Related entity names
              CategoryName = p.Category?.CategoryName ?? string.Empty,
              BrandName = p.Brand?.BrandName ?? string.Empty,
              GenericName = p.Generic?.GenericName ?? string.Empty,
              FormulationName = p.Formulation?.FormulationName ?? string.Empty,
              ClassificationName = p.Classification?.ClassificationName ?? string.Empty,
              TypeName = p.TypeModel?.TypeName ?? string.Empty,
              }).ToList()
          });
    }

    public async Task<SupplierDTO> GetSupplierByIdAsync(int id)
    {
      var s = await _getRepo.GetSupplierByIdAsync(id);
      if (s == null)
        return null;
      return new SupplierDTO
      {
        Id = s.Id,
        SupplierName = s.SupplierName,
        ContactNumber = s.ContactNumber,
        ContactPerson = s.ContactPerson,
        ContactPersonNumber = s.ContactPersonNumber,
        Email = s.Email,
        Address = s.Address,
        IsActive = s.IsActive
      };
    }
    #endregion

    #region Inventory 
    public async Task<IEnumerable<InventoryDto>> GetAllInventoryAsync()
    {
      // Get inventories from repository (ensure repo returns Product navigation if possible)
      var inventories = await _getRepo.GetAllInventoryAsync();

      // Deactivate inventories with invalid total pieces (existing behavior)
      foreach (var inv in inventories)
      {
        if (inv.TotalPiecesAvailable < 0)
        {
          inv.IsActive = false;
          await _updateRepo.UpdateInventoryAsync(inv);
        }
      }

      // Map to DTOs (only fields present on InventoryDto)
      var result = inventories.Select(i => new InventoryDto
          {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,

          // Map nested Product -> ProductDto (safe null checks)
          Product = i.Product == null ? null : new ProductDto
          {
          ProductID = i.Product.ProductID,
          ProductName = i.Product.ProductName,
          CategoryID = i.Product.CategoryID,
          BrandID = i.Product.BrandID,
          GenericID = i.Product.GenericID,
          FormulationID = i.Product.FormulationID,
          ClassificationID = i.Product.ClassificationID,
          TypeID = i.Product.TypeID,
          IsActive = i.Product.IsActive,
          DateCreated = i.Product.DateCreated,
          SheetsPerBox = i.Product.SheetsPerBox,
          PiecesPerSheet = i.Product.PiecesPerSheet,
          BoxPrice = i.Product.BoxPrice,
          SheetPrice = i.Product.SheetPrice,
          PiecePrice = i.Product.PiecePrice,
          ReorderLevel = i.Product.ReorderLevel,
          UnitMeasurement = i.Product.UnitMeasurement,
          PiecePerBox = i.PiecePerBox,

          // Derived product properties (guarded against null Inventory collections)
          ExpiredStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
          NearExpiryStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsNearExpiry && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
          IsCriticalStock = (i.Product.Inventory?.Where(x => x.IsActive && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0) <= i.Product.ReorderLevel,
          HasExpiredStock = i.Product.Inventory?.Any(x => x.IsActive && x.IsExpired) ?? false,

          // Related names
          CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
          BrandName = i.Product.Brand?.BrandName ?? string.Empty,
          GenericName = i.Product.Generic?.GenericName ?? string.Empty,
          FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
          ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
          TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
          },

          // Inventory-specific fields
          ProductName = i.Product?.ProductName ?? string.Empty,
          CategoryName = i.Product?.Category?.CategoryName ?? string.Empty,  // ADD THIS LINE
          BrandName = i.Product?.Brand?.BrandName ?? string.Empty,           // ADD THIS LINE
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,

          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,

          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,

          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,

          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,

          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable
          })
      .ToList();

      return result;
    }
    public async Task<List<InventoryDto>> GetAllAvailableInventoryAsync()
    {
      var inventories = await _getRepo.GetAllAvailableInventoryAsync();
      if (inventories.Count() == 0)
        return new List<InventoryDto>();
      return inventories.Select(i => new InventoryDto
          {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,
          PiecePerBox = i.PiecePerBox,

          // Map nested Product -> ProductDto (safe null checks)
          Product = i.Product == null ? null : new ProductDto
          {
          ProductID = i.Product.ProductID,
          ProductName = i.Product.ProductName,
          CategoryID = i.Product.CategoryID,
          BrandID = i.Product.BrandID,
          GenericID = i.Product.GenericID,
          FormulationID = i.Product.FormulationID,
          ClassificationID = i.Product.ClassificationID,
          TypeID = i.Product.TypeID,
          IsActive = i.Product.IsActive,
          DateCreated = i.Product.DateCreated,
          SheetsPerBox = i.Product.SheetsPerBox,
          PiecesPerSheet = i.Product.PiecesPerSheet,
          BoxPrice = i.Product.BoxPrice,
          SheetPrice = i.Product.SheetPrice,
          PiecePrice = i.Product.PiecePrice,
          ReorderLevel = i.Product.ReorderLevel,
          UnitMeasurement = i.Product.UnitMeasurement,
          PiecePerBox = i.PiecePerBox,

          // Derived product properties (guarded against null Inventory collections)
          ExpiredStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
          NearExpiryStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsNearExpiry && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
          IsCriticalStock = (i.Product.Inventory?.Where(x => x.IsActive && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0) <= i.Product.ReorderLevel,
          HasExpiredStock = i.Product.Inventory?.Any(x => x.IsActive && x.IsExpired) ?? false,

          // Related names
          CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
          BrandName = i.Product.Brand?.BrandName ?? string.Empty,
          GenericName = i.Product.Generic?.GenericName ?? string.Empty,
          FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
          ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
          TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
          },

          // Inventory-specific fields
          ProductName = i.Product?.ProductName ?? string.Empty,
          CategoryName = i.Product?.Category?.CategoryName ?? string.Empty,  // ADD THIS LINE
          BrandName = i.Product?.Brand?.BrandName ?? string.Empty,           // ADD THIS LINE
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,

          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,

          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,

          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,

          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,

          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable,
          DiscountId = i.DiscountId,
          })
      .ToList();
    }

    public async Task<InventoryDto?> GetInventoryByIdAsync(int id)
    {
      var i = await _getRepo.GetInventoryByIdAsync(id);
      if (i == null)
        return null;
      return new InventoryDto
      {
        InventoryID = i.InventoryID,
        ProductID = i.ProductID,
        ProductName = i.Product?.ProductName ?? string.Empty,
        BatchNumber = i.BatchNumber,
        ManufacturingDate = i.ManufacturingDate,
        ExpirationDate = i.ExpirationDate,
        BoxQuantity = i.BoxQuantity,
        SheetQuantity = i.SheetQuantity,
        PieceQuantity = i.PieceQuantity,
        SheetsPerBox = i.SheetsPerBox,
        PiecesPerSheet = i.PiecesPerSheet,
        ReorderLevel = i.ReorderLevel,
        CostPrice = i.CostPrice,
        BoxPrice = i.BoxPrice,
        SheetPrice = i.SheetPrice,
        PiecePrice = i.PiecePrice,
        DateAdded = i.DateAdded,
        IsActive = i.IsActive,
        IsExpired = i.IsExpired,
        IsNearExpiry = i.IsNearExpiry,
        IsCriticalStock = i.IsCriticalStock,
        IsMultiUnit = i.IsMultiUnit,
        TotalPiecesAvailable = i.TotalPiecesAvailable
      };
    }

    public async Task<IEnumerable<InventoryDto>> GetInventoryByProductIdAsync(int productId)
    {
      var inventories = await _getRepo.GetInventoryByProductIdAsync(productId);
      return inventories.Select(i => new InventoryDto
          {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,
          ProductName = i.Product?.ProductName ?? string.Empty,
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,
          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,
          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,
          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,
          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,
          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable
          });
    }

    public async Task<IEnumerable<InventoryDto>> GetExpiredInventoryAsync()
    {
      var inventories = await _getRepo.GetExpiredInventoryAsync();
      return inventories.Select(i => new InventoryDto
          {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,
          ProductName = i.Product?.ProductName ?? string.Empty,
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,
          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,
          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,
          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,
          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,
          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable,
          IsDeleted = i.IsDeleted,
          });
    }

    public async Task<IEnumerable<InventoryDto>> GetNearExpiryInventoryAsync()
    {
      var inventories = await _getRepo.GetNearExpiryInventoryAsync();
      return inventories.Select(i => new InventoryDto
          {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,
          ProductName = i.Product?.ProductName ?? string.Empty,
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,
          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,
          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,
          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,
          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,
          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable,
          Product = new ProductDto
          {
            ProductID = i.Product.ProductID,
            ProductName = i.Product.ProductName,
            CategoryID = i.Product.CategoryID,
            BrandID = i.Product.BrandID,
            GenericID = i.Product.GenericID,
            FormulationID = i.Product.FormulationID,
            ClassificationID = i.Product.ClassificationID,
            TypeID = i.Product.TypeID,
            IsActive = i.Product.IsActive,
            DateCreated = i.Product.DateCreated,
            SheetsPerBox = i.Product.SheetsPerBox,
            PiecesPerSheet = i.Product.PiecesPerSheet,
            BoxPrice = i.Product.BoxPrice,
            SheetPrice = i.Product.SheetPrice,
            PiecePrice = i.Product.PiecePrice,
            ReorderLevel = i.Product.ReorderLevel,
            UnitMeasurement = i.Product.UnitMeasurement,
            PiecePerBox = i.Product.PiecePerBox,

            // Derived product properties (guarded against null Inventory collections)
            ExpiredStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
            NearExpiryStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsNearExpiry && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
            IsCriticalStock = (i.Product.Inventory?.Where(x => x.IsActive && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0) <= i.Product.ReorderLevel,
            HasExpiredStock = i.Product.Inventory?.Any(x => x.IsActive && x.IsExpired) ?? false,

            // Related names
            CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
            BrandName = i.Product.Brand?.BrandName ?? string.Empty,
            GenericName = i.Product.Generic?.GenericName ?? string.Empty,
            FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
            ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
            TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
          }
          });
    }

    public async Task<IEnumerable<InventoryDto>> GetCriticalStockInventoryAsync()
    {
      var inventories = await _getRepo.GetCriticalStockInventoryAsync();
      return inventories.Select(i => new InventoryDto
          {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,
          ProductName = i.Product?.ProductName ?? string.Empty,
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,
          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,
          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,
          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,
          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,
          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable,
          Product = new ProductDto
          {
            ProductID = i.Product.ProductID,
            ProductName = i.Product.ProductName,
            CategoryID = i.Product.CategoryID,
            BrandID = i.Product.BrandID,
            GenericID = i.Product.GenericID,
            FormulationID = i.Product.FormulationID,
            ClassificationID = i.Product.ClassificationID,
            TypeID = i.Product.TypeID,
            IsActive = i.Product.IsActive,
            DateCreated = i.Product.DateCreated,
            SheetsPerBox = i.Product.SheetsPerBox,
            PiecesPerSheet = i.Product.PiecesPerSheet,
            BoxPrice = i.Product.BoxPrice,
            SheetPrice = i.Product.SheetPrice,
            PiecePrice = i.Product.PiecePrice,
            ReorderLevel = i.Product.ReorderLevel,
            UnitMeasurement = i.Product.UnitMeasurement,
            PiecePerBox = i.Product.PiecePerBox,

            // Derived product properties (guarded against null Inventory collections)
            ExpiredStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
            NearExpiryStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsNearExpiry && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
            IsCriticalStock = (i.Product.Inventory?.Where(x => x.IsActive && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0) <= i.Product.ReorderLevel,
            HasExpiredStock = i.Product.Inventory?.Any(x => x.IsActive && x.IsExpired) ?? false,

            // Related names
            CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
            BrandName = i.Product.Brand?.BrandName ?? string.Empty,
            GenericName = i.Product.Generic?.GenericName ?? string.Empty,
            FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
            ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
            TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
          }
          });
    }

    public async Task<List<InventoryDto>> GetInventoriesOutOfStockAsync() 
    {
      var inventories = await _getRepo.GetInventoriesOutOfStockAsync();
      if(!inventories.Any() || inventories.Count() == 0) 
        return new List<InventoryDto>();

      var modifInventories = inventories.Select(i => new InventoryDto {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,
          ProductName = i.Product?.ProductName ?? string.Empty,
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,
          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,
          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,
          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,
          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,
          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable,
          Product = new ProductDto
          {
            ProductID = i.Product.ProductID,
            ProductName = i.Product.ProductName,
            CategoryID = i.Product.CategoryID,
            BrandID = i.Product.BrandID,
            GenericID = i.Product.GenericID,
            FormulationID = i.Product.FormulationID,
            ClassificationID = i.Product.ClassificationID,
            TypeID = i.Product.TypeID,
            IsActive = i.Product.IsActive,
            DateCreated = i.Product.DateCreated,
            SheetsPerBox = i.Product.SheetsPerBox,
            PiecesPerSheet = i.Product.PiecesPerSheet,
            BoxPrice = i.Product.BoxPrice,
            SheetPrice = i.Product.SheetPrice,
            PiecePrice = i.Product.PiecePrice,
            ReorderLevel = i.Product.ReorderLevel,
            UnitMeasurement = i.Product.UnitMeasurement,
            PiecePerBox = i.Product.PiecePerBox,

            // Derived product properties (guarded against null Inventory collections)
            ExpiredStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
            NearExpiryStock = i.Product.Inventory?.Where(x => x.IsActive && x.IsNearExpiry && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0,
            IsCriticalStock = (i.Product.Inventory?.Where(x => x.IsActive && !x.IsExpired).Sum(x => x.BoxQuantity) ?? 0) <= i.Product.ReorderLevel,
            HasExpiredStock = i.Product.Inventory?.Any(x => x.IsActive && x.IsExpired) ?? false,

            // Related names
            CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
            BrandName = i.Product.Brand?.BrandName ?? string.Empty,
            GenericName = i.Product.Generic?.GenericName ?? string.Empty,
            FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
            ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
            TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
          }
      }).ToList();
      return modifInventories;
    }

    #endregion

    #region VAT
    public async Task<IEnumerable<VatDto>> GetAllVATAsync()
    {
      var vats = await _getRepo.GetAllVATAsync();
      return vats.Select(v => new VatDto
          {
          VATID = v.VATID,
          VATName = v.VATName,
          VATRate = v.VATRate,
          IsActive = v.IsActive,
          DateCreated = v.DateCreated
          });
    }

    public async Task<VatDto?> GetVATByIdAsync(int id)
    {
      var v = await _getRepo.GetVATByIdAsync(id);
      if (v == null)
        return null;
      return new VatDto
      {
        VATID = v.VATID,
        VATName = v.VATName,
        VATRate = v.VATRate,
        IsActive = v.IsActive,
        DateCreated = v.DateCreated
      };
    }
    #endregion

    #region PurchaseOrder
    public async Task<IEnumerable<PurchaseOrderDTO>> GetAllPurchaseOrderAsync()
    {
      var orders = await _getRepo.GetAllPurchaseOrderAsync();
      return orders.Select(po => new PurchaseOrderDTO
          {
          Id = po.Id,
          PurchaseOrderNumber = po.PurchaseOrderNumber,
          OrderDate = po.OrderDate,
          DeliveryDate = po.DeliveryDate,
          SupplierName = po.Supplier?.SupplierName ?? string.Empty,
          TotalAmount = po.TotalAmount,
          Status = po.Status,
          Notes = po.Notes,
          PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
              {
              Id = poi.Id,
              ProductId = poi.ProductID,
              ProductName = poi.Product?.ProductName ?? string.Empty,
              BoxQuantityOrdered = poi.BoxQuantityOrdered,
              BoxQuantityReceived = poi.BoxQuantityReceived,
              SheetQuantityOrdered = poi.SheetQuantityOrdered,
              SheetQuantityReceived = poi.SheetQuantityReceived,
              PieceQuantityOrdered = poi.PieceQuantityOrdered,
              PieceQuantityReceived = poi.PieceQuantityReceived,
              BatchNumber = poi.BatchNumber,
              BoxPrice  = poi.BoxPrice,
              SheetPrice = poi.SheetPrice,
              PiecePrice = poi.PiecePrice,
              TotalCost = poi.TotalCost,
              PoUnitType = poi.PoUnitType,
              }).ToList()
          });
    }

    public async Task<PurchaseOrderDTO?> GetPurchaseOrderByIdAsync(int id)
    {
      var po = await _getRepo.GetPurchaseOrderByIdAsync(id);
      if (po == null)
        return null;
      return new PurchaseOrderDTO
      {
        Id = po.Id,
        PurchaseOrderNumber = po.PurchaseOrderNumber,
        OrderDate = po.OrderDate,
        DeliveryDate = po.DeliveryDate,
        SupplierName = po.Supplier?.SupplierName ?? string.Empty,
        TotalAmount = po.TotalAmount,
        Status = po.Status,
        Notes = po.Notes,
        PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
            {
            Id = poi.Id,
            ProductId = poi.ProductID,
            ProductName = poi.Product?.ProductName ?? string.Empty,
            BoxQuantityOrdered = poi.BoxQuantityOrdered,
            BoxQuantityReceived = poi.BoxQuantityReceived,
            SheetQuantityOrdered = poi.SheetQuantityOrdered,
            SheetQuantityReceived = poi.SheetQuantityReceived,
            PieceQuantityOrdered = poi.PieceQuantityOrdered,
            PieceQuantityReceived = poi.PieceQuantityReceived,
            BatchNumber = poi.BatchNumber,
            BoxPrice = poi.BoxPrice,
            SheetPrice = poi.SheetPrice,
            PiecePrice = poi.PiecePrice,
            TotalCost = poi.TotalCost,
            PoUnitType = poi.PoUnitType,
            }).ToList()
      };
    }

    public async Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderBySupplierIdAsync(int supplierId)
    {
      var orders = await _getRepo.GetPurchaseOrderBySupplierIdAsync(supplierId);
      return orders.Select(po => new PurchaseOrderDTO
          {
          Id = po.Id,
          PurchaseOrderNumber = po.PurchaseOrderNumber,
          OrderDate = po.OrderDate,
          DeliveryDate = po.DeliveryDate,
          SupplierName = po.Supplier?.SupplierName ?? string.Empty,
          TotalAmount = po.TotalAmount,
          Status = po.Status,
          Notes = po.Notes,
          PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
              {
              Id = poi.Id,
              ProductId = poi.ProductID,
              ProductName = poi.Product?.ProductName ?? string.Empty,
              BoxQuantityOrdered = poi.BoxQuantityOrdered,
              BoxQuantityReceived = poi.BoxQuantityReceived,
              SheetQuantityOrdered = poi.SheetQuantityOrdered,
              SheetQuantityReceived = poi.SheetQuantityReceived,
              PieceQuantityOrdered = poi.PieceQuantityOrdered,
              PieceQuantityReceived = poi.PieceQuantityReceived,
              BatchNumber = poi.BatchNumber,
              BoxPrice = poi.BoxPrice,
              SheetPrice = poi.SheetPrice,
              PiecePrice = poi.PiecePrice,
              TotalCost = poi.TotalCost
              }).ToList()
          });
    }

    public async Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderByStatusAsync(string status)
    {
      var orders = await _getRepo.GetPurchaseOrderByStatusAsync(status);
      return orders.Select(po => new PurchaseOrderDTO
          {
          Id = po.Id,
          PurchaseOrderNumber = po.PurchaseOrderNumber,
          OrderDate = po.OrderDate,
          DeliveryDate = po.DeliveryDate,
          SupplierName = po.Supplier.SupplierName,
          TotalAmount = po.TotalAmount,
          Status = po.Status,
          Notes = po.Notes,
          PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
              {
              Id = poi.Id,
              ProductId = poi.ProductID,
              ProductName = poi.Product?.ProductName ?? string.Empty,
              BoxQuantityOrdered = poi.BoxQuantityOrdered,
              BoxQuantityReceived = poi.BoxQuantityReceived,
              SheetQuantityOrdered = poi.SheetQuantityOrdered,
              SheetQuantityReceived = poi.SheetQuantityReceived,
              PieceQuantityOrdered = poi.PieceQuantityOrdered,
              PieceQuantityReceived = poi.PieceQuantityReceived,
              BatchNumber = poi.BatchNumber,
              BoxPrice = poi.BoxPrice,
              SheetPrice = poi.SheetPrice,
              PiecePrice = poi.PiecePrice,
              TotalCost = poi.TotalCost,
              PoUnitType = poi.PoUnitType,
              }).ToList()
          });
    }
    public async Task<List<PurchaseOrderDTO>> GetPendingPurchaseOrdersAsync()
    {
      var purchaseOrders = await _getRepo.GetPendingPurchaseOrdersAsync();
      if (!purchaseOrders.Any() || purchaseOrders.Count() == 0)
        return new List<PurchaseOrderDTO>();

      var modifPurchaseOrders = purchaseOrders.Select(po => new PurchaseOrderDTO
          {
          Id = po.Id,
          PurchaseOrderNumber = po.PurchaseOrderNumber,
          OrderDate = po.OrderDate,
          DeliveryDate = po.DeliveryDate,
          SupplierName = po.Supplier.SupplierName,
          TotalAmount = po.TotalAmount,
          Status = po.Status,
          Notes = po.Notes,
          PurchaseOrderItems = po.PurchaseOrderItems.Select(poi => new PurchaseOrderItemDto
              {
              Id = poi.Id,
              ProductId = poi.ProductID,
              ProductName = poi.Product?.ProductName ?? string.Empty,
              BoxQuantityOrdered = poi.BoxQuantityOrdered,
              BoxQuantityReceived = poi.BoxQuantityReceived,
              SheetQuantityOrdered = poi.SheetQuantityOrdered,
              SheetQuantityReceived = poi.SheetQuantityReceived,
              PieceQuantityOrdered = poi.PieceQuantityOrdered,
              PieceQuantityReceived = poi.PieceQuantityReceived,
              BatchNumber = poi.BatchNumber,
              BoxPrice = poi.BoxPrice,
              SheetPrice = poi.SheetPrice,
              PiecePrice = poi.PiecePrice,
              TotalCost = poi.TotalCost,
              PoUnitType = poi.PoUnitType,
              }).ToList()
          }).ToList();
      return modifPurchaseOrders;
    }
    #endregion

    #region StockMovement
    public async Task<IEnumerable<StockMovementDto>> GetAllStockMovementAsync()
    {
      var movements = await _getRepo.GetAllStockMovementAsync();
      return movements.Select(sm => new StockMovementDto
          {
          StockMovementID = sm.StockMovementID,
          InventoryID = sm.InventoryID,
          ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
          BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
          MovementType = sm.MovementType,
          BoxQuantityChanged = sm.BoxQuantityChanged,
          SheetQuantityChanged = sm.SheetQuantityChanged,
          PieceQuantityChanged = sm.PieceQuantityChanged,
          BoxQuantityBefore = sm.BoxQuantityBefore,
          SheetQuantityBefore = sm.SheetQuantityBefore,
          PieceQuantityBefore = sm.PieceQuantityBefore,
          BoxQuantityAfter = sm.BoxQuantityAfter,
          SheetQuantityAfter = sm.SheetQuantityAfter,
          PieceQuantityAfter = sm.PieceQuantityAfter,
          ReferenceNumber = sm.ReferenceNumber,
          Notes = sm.Notes,
          MovementDate = sm.MovementDate,
          EmployeeID = sm.EmployeeID,
          EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
          });
    }

    public async Task<StockMovementDto?> GetStockMovementByIdAsync(int id)
    {
      var sm = await _getRepo.GetStockMovementByIdAsync(id);
      if (sm == null)
        return null;
      return new StockMovementDto
      {
        StockMovementID = sm.StockMovementID,
        InventoryID = sm.InventoryID,
        ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
        BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
        MovementType = sm.MovementType,
        BoxQuantityChanged = sm.BoxQuantityChanged,
        SheetQuantityChanged = sm.SheetQuantityChanged,
        PieceQuantityChanged = sm.PieceQuantityChanged,
        BoxQuantityBefore = sm.BoxQuantityBefore,
        SheetQuantityBefore = sm.SheetQuantityBefore,
        PieceQuantityBefore = sm.PieceQuantityBefore,
        BoxQuantityAfter = sm.BoxQuantityAfter,
        SheetQuantityAfter = sm.SheetQuantityAfter,
        PieceQuantityAfter = sm.PieceQuantityAfter,
        ReferenceNumber = sm.ReferenceNumber,
        Notes = sm.Notes,
        MovementDate = sm.MovementDate,
        EmployeeID = sm.EmployeeID,
        EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
      };
    }

    public async Task<IEnumerable<StockMovementDto>> GetStockMovementByInventoryIdAsync(int inventoryId)
    {
      var movements = await _getRepo.GetStockMovementByInventoryIdAsync(inventoryId);
      return movements.Select(sm => new StockMovementDto
          {
          StockMovementID = sm.StockMovementID,
          InventoryID = sm.InventoryID,
          ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
          BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
          MovementType = sm.MovementType,
          BoxQuantityChanged = sm.BoxQuantityChanged,
          SheetQuantityChanged = sm.SheetQuantityChanged,
          PieceQuantityChanged = sm.PieceQuantityChanged,
          BoxQuantityBefore = sm.BoxQuantityBefore,
          SheetQuantityBefore = sm.SheetQuantityBefore,
          PieceQuantityBefore = sm.PieceQuantityBefore,
          BoxQuantityAfter = sm.BoxQuantityAfter,
          SheetQuantityAfter = sm.SheetQuantityAfter,
          PieceQuantityAfter = sm.PieceQuantityAfter,
          ReferenceNumber = sm.ReferenceNumber,
          Notes = sm.Notes,
          MovementDate = sm.MovementDate,
          EmployeeID = sm.EmployeeID,
          EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
          });
    }

    public async Task<IEnumerable<StockMovementDto>> GetStockMovementByProductIdAsync(int productId)
    {
      var movements = await _getRepo.GetStockMovementByProductIdAsync(productId);
      return movements.Select(sm => new StockMovementDto
          {
          StockMovementID = sm.StockMovementID,
          InventoryID = sm.InventoryID,
          ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
          BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
          MovementType = sm.MovementType,
          BoxQuantityChanged = sm.BoxQuantityChanged,
          SheetQuantityChanged = sm.SheetQuantityChanged,
          PieceQuantityChanged = sm.PieceQuantityChanged,
          BoxQuantityBefore = sm.BoxQuantityBefore,
          SheetQuantityBefore = sm.SheetQuantityBefore,
          PieceQuantityBefore = sm.PieceQuantityBefore,
          BoxQuantityAfter = sm.BoxQuantityAfter,
          SheetQuantityAfter = sm.SheetQuantityAfter,
          PieceQuantityAfter = sm.PieceQuantityAfter,
          ReferenceNumber = sm.ReferenceNumber,
          Notes = sm.Notes,
          MovementDate = sm.MovementDate,
          EmployeeID = sm.EmployeeID,
          EmployeeName = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty
          });
    }
    #endregion

    #region Reports

    public async Task<SalesReportDto> GetSalesReportAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? employeeId = null,
        string paymentMethod = null)
    {
      var transactions = await _getRepo.GetSalesReportDataAsync(
          startDate,
          endDate,
          employeeId,
          paymentMethod);

      var transactionDtos = transactions.Select(t => new SalesTransactionDto
          {
          Id = t.Id,
          TransactionDate = t.TransactionDate,
          InvoiceNumber = t.InvoiceNumber,
          TotalAmount = t.TotalAmount,
          DiscountAmount = t.DiscountAmount,
          VATAmount = t.VATAmount,
          NetAmount = t.NetAmount,
          PaymentMethod = t.PaymentMethod,
          AmountPaid = t.AmountPaid,
          ChangeAmount = t.ChangeAmount,
          DiscountId = t.DiscountId,
          EmployeeID = t.EmployeeID,
          VATId = t.VATId,
          DiscountName = t.Discount?.DiscountName ?? string.Empty,
          EmployeeName = t.Employee != null ? $"{t.Employee.FirstName} {t.Employee.LastName}" : string.Empty,
          VATRate = t.VAT?.VATRate,
          Items = t.Items.Select(i => new SalesItemDto
              {
              Id = i.Id,
              SalesTransactionId = i.SalesTransactionId,
              ProductId = i.ProductId,
              ProductName = i.Product?.ProductName ?? string.Empty,
              UnitType = i.UnitType,
              Quantity = i.Quantity,
              UnitPrice = i.UnitPrice,
              Subtotal = i.Subtotal
              }).ToList()
          }).ToList();

      var summary = new SalesReportSummaryDto
      {
        TotalTransactions = transactionDtos.Count(),
        NetRevenue = transactionDtos.Sum(t => t.NetAmount),
        TotalItemsSold = transactionDtos.Sum(t => t.Items.Sum(i => i.Quantity)),
        AverageTransaction = transactionDtos.Any()
          ? transactionDtos.Average(t => t.NetAmount)
          : 0
      };

      return new SalesReportDto
      {
        Summary = summary,
        Transactions = transactionDtos
      };
    }

    //Inventory-Reports

    public async Task<InventoryReportDto> GetInventoryReportAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string filterType = "all")
    {
      var inventories = await _getRepo.GetInventoryReportDataAsync(
          startDate,
          endDate,
          filterType);

      var inventoryDtos = inventories.Select(i => new InventoryDto
          {
          InventoryID = i.InventoryID,
          ProductID = i.ProductID,
          ProductName = i.Product?.ProductName ?? string.Empty,
          BatchNumber = i.BatchNumber,
          ManufacturingDate = i.ManufacturingDate,
          ExpirationDate = i.ExpirationDate,
          BoxQuantity = i.BoxQuantity,
          SheetQuantity = i.SheetQuantity,
          PieceQuantity = i.PieceQuantity,
          SheetsPerBox = i.SheetsPerBox,
          PiecesPerSheet = i.PiecesPerSheet,
          ReorderLevel = i.ReorderLevel,
          CostPrice = i.CostPrice,
          BoxPrice = i.BoxPrice,
          SheetPrice = i.SheetPrice,
          PiecePrice = i.PiecePrice,
          DateAdded = i.DateAdded,
          IsActive = i.IsActive,
          IsExpired = i.IsExpired,
          IsNearExpiry = i.IsNearExpiry,
          IsCriticalStock = i.IsCriticalStock,
          IsMultiUnit = i.IsMultiUnit,
          TotalPiecesAvailable = i.TotalPiecesAvailable,
          Product = i.Product == null ? null : new ProductDto
          {
            ProductID = i.Product.ProductID,
            ProductName = i.Product.ProductName,
            CategoryName = i.Product.Category?.CategoryName ?? string.Empty,
            BrandName = i.Product.Brand?.BrandName ?? string.Empty,
            GenericName = i.Product.Generic?.GenericName ?? string.Empty,
            FormulationName = i.Product.Formulation?.FormulationName ?? string.Empty,
            ClassificationName = i.Product.Classification?.ClassificationName ?? string.Empty,
            TypeName = i.Product.TypeModel?.TypeName ?? string.Empty
          }
          }).ToList();

      var summary = new InventoryReportSummaryDto
      {
        TotalProducts = inventoryDtos.Select(i => i.ProductID).Distinct().Count(),
        TotalValue = inventoryDtos.Sum(i => i.TotalPiecesAvailable * i.PiecePrice),
        CriticalStockCount = inventoryDtos.Count(i => i.IsCriticalStock),
        NearExpiryCount = inventoryDtos.Count(i => i.IsNearExpiry && !i.IsExpired)
      };

      return new InventoryReportDto
      {
        Summary = summary,
        InventoryItems = inventoryDtos
      };
    }

        //Fast-Moving Products

        public async Task<FastMovingProductReportDto> GetFastMovingProductsReportAsync(
    DateTime? startDate = null,
    DateTime? endDate = null,
    int topN = 20)
        {
            var productsData = await _getRepo.GetFastMovingProductsDataAsync(
                startDate,
                endDate,
                topN);

            // Convert to list to avoid multiple enumeration
            var productsDataList = productsData.ToList();
            var products = new List<FastMovingProductDto>();

            for (int i = 0; i < productsDataList.Count; i++)
            {
                var productData = productsDataList[i];

                var product = new FastMovingProductDto
                {
                    Rank = i + 1,
                    ProductId = productData.ProductId,
                    ProductName = productData.ProductName,
                    Brand = productData.Brand,
                    CategoryName = productData.CategoryName,
                    Generic = productData.Generic,
                    Classification = productData.Classification,
                    UnitType = productData.UnitType,
                    QuantitySold = productData.QuantitySold, // This is now available
                    TransactionCount = productData.TransactionCount,
                    Revenue = productData.Revenue,
                    CurrentStock = productData.CurrentStock,
                    StockStatus = DetermineStockStatus(productData.CurrentStock),
                    Trend = DetermineTrend(productData.QuantitySold, productData.CurrentStock)
                };

                // Calculate Box, Sheet, Piece sold based on unit type
                CalculateSoldQuantities(product, productData);

                products.Add(product);
            }

            return new FastMovingProductReportDto
            {
                TotalProductsSold = products.Count,
                TotalQuantitySold = products.Sum(p => p.QuantitySold),
                TotalRevenue = products.Sum(p => p.Revenue),
                Products = products
            };
        }

        private void CalculateSoldQuantities(FastMovingProductDto product, FastMovingProductDataDto productData)
        {
            int totalPiecesSold = productData.QuantitySold;

            switch (productData.UnitType?.ToLower())
            {
                case "multi-level":
                    if (productData.SheetsPerBox.HasValue && productData.SheetsPerBox > 0 &&
                        productData.PiecesPerSheet.HasValue && productData.PiecesPerSheet > 0)
                    {
                        int piecesPerBox = productData.SheetsPerBox.Value * productData.PiecesPerSheet.Value;
                        product.BoxSold = totalPiecesSold / piecesPerBox;
                        int remainder = totalPiecesSold % piecesPerBox;
                        product.SheetSold = remainder / productData.PiecesPerSheet.Value;
                        product.PieceSold = totalPiecesSold; // Total accumulated pieces
                    }
                    else
                    {
                        product.PieceSold = totalPiecesSold;
                    }
                    break;

                case "dual-level":
                    if (productData.PiecePerBox.HasValue && productData.PiecePerBox > 0)
                    {
                        product.BoxSold = totalPiecesSold / productData.PiecePerBox.Value;
                        product.PieceSold = totalPiecesSold; // Total accumulated pieces
                    }
                    else
                    {
                        product.PieceSold = totalPiecesSold;
                    }
                    break;

                case "single-level":
                default:
                    product.PieceSold = totalPiecesSold;
                    break;
            }
        }

        private string DetermineStockStatus(int currentStock)
        {
            if (currentStock == 0) return "Out of Stock";
            if (currentStock < 10) return "Low Stock";
            return "Adequate";
        }

        private string DetermineTrend(int quantitySold, int currentStock)
        {
            if (currentStock == 0) return "Decreasing";

            var ratio = (double)quantitySold / currentStock;

            if (ratio > 2.0) return "Increasing";
            if (ratio > 0.5) return "Stable";
            return "Decreasing";
        }


        #endregion

        #region Reports Exports In excell

        //SALES REPORTs

        public async Task<byte[]> ExportSalesReportToExcelAsync(
    DateTime? startDate = null,
    DateTime? endDate = null,
    int? employeeId = null,
    string paymentMethod = null,
    int? currentLoggedInUserId = null)
        {
            var report = await GetSalesReportAsync(startDate, endDate, employeeId, paymentMethod);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sales Report");

                // Get the current logged-in user's name
                string adminName = "Administrator";
                if (currentLoggedInUserId.HasValue && currentLoggedInUserId.Value > 0)
                {
                    var currentUser = await GetEmployeeByIdAsync(currentLoggedInUserId.Value);
                    adminName = currentUser?.FullName ?? "Administrator";
                }

                string pharmacyAddress = "Racquel Pharmacy";
                string printDate = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

                // ===== HEADER SECTION =====
                int currentRow = 1;
                int headerStartRow = currentRow;

                // SALES REPORT - Title (Centered)
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "SALES REPORT";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 20;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 120));
                worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 35;

                currentRow++;

                // Pharmacy Name - Centered
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Racquel Pharmacy";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 25;

                currentRow++;

                // Pharmacy Address - Centered
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Q3H+HGC, Makabud, Novaliches, Caloocan, Metro Manila";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Empty row for spacing with background
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 5;

                currentRow++;

                // Period - Centered
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Period: " + (startDate.HasValue && endDate.HasValue
                  ? $"{startDate:MMM dd, yyyy} to {endDate:MMM dd, yyyy}"
                  : "All Time");
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Printed By - Centered
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Printed By: " + adminName;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Print Date - Centered
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Print Date: " + printDate;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                // Employee Filter (if applicable)
                if (employeeId.HasValue)
                {
                    currentRow++;
                    var employee = await GetEmployeeByIdAsync(employeeId.Value);
                    worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                    worksheet.Cells[$"A{currentRow}"].Value = "Cashier: " + (employee?.FullName ?? "N/A");
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                    worksheet.Row(currentRow).Height = 20;
                }

                // Payment Method Filter (if applicable)
                if (!string.IsNullOrEmpty(paymentMethod) && paymentMethod.ToLower() != "all methods")
                {
                    currentRow++;
                    worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                    worksheet.Cells[$"A{currentRow}"].Value = "Payment Method: " + paymentMethod;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                    worksheet.Row(currentRow).Height = 20;
                }

                // Add border around entire header section
                using (var range = worksheet.Cells[headerStartRow, 1, currentRow, 10])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }

                currentRow += 2;

                // ===== SUMMARY SECTION =====
                int summaryStartRow = currentRow;

                // Summary Headers with styling - Extended to column J to match table width
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Row(currentRow).Height = 25;

                // Merge cells for each summary metric to make them wider
                worksheet.Cells[$"A{currentRow}:B{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Total Transactions";

                worksheet.Cells[$"C{currentRow}:E{currentRow}"].Merge = true;
                worksheet.Cells[$"C{currentRow}"].Value = "Net Revenue";

                worksheet.Cells[$"F{currentRow}:H{currentRow}"].Merge = true;
                worksheet.Cells[$"F{currentRow}"].Value = "Total Items Sold";

                worksheet.Cells[$"I{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"I{currentRow}"].Value = "Average Transaction";

                currentRow++;

                // Summary Values
                worksheet.Cells[$"A{currentRow}:B{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = report.Summary.TotalTransactions;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 30;

                worksheet.Cells[$"C{currentRow}:E{currentRow}"].Merge = true;
                worksheet.Cells[$"C{currentRow}"].Value = report.Summary.NetRevenue;
                worksheet.Cells[$"C{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                worksheet.Cells[$"C{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"C{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"C{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"C{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"C{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 60));
                worksheet.Cells[$"C{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"C{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));

                worksheet.Cells[$"F{currentRow}:H{currentRow}"].Merge = true;
                worksheet.Cells[$"F{currentRow}"].Value = report.Summary.TotalItemsSold;
                worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"F{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"F{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"F{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"F{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"F{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                worksheet.Cells[$"I{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"I{currentRow}"].Value = report.Summary.AverageTransaction;
                worksheet.Cells[$"I{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                worksheet.Cells[$"I{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"I{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"I{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"I{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"I{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"I{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                // Add border to summary section - Extended to column J
                using (var range = worksheet.Cells[summaryStartRow, 1, currentRow, 10])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }

                // ===== TRANSACTIONS SECTION =====
                currentRow += 3;
                int transactionsStartRow = currentRow;

                // Section Title
                worksheet.Cells[$"A{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "TRANSACTION DETAILS";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 120));
                worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 25;

                currentRow++;

                // Transaction Headers
                worksheet.Cells[$"A{currentRow}"].Value = "Invoice #";
                worksheet.Cells[$"B{currentRow}"].Value = "Date & Time";
                worksheet.Cells[$"C{currentRow}"].Value = "Product Name";
                worksheet.Cells[$"D{currentRow}"].Value = "Unit Type";
                worksheet.Cells[$"E{currentRow}"].Value = "Quantity";
                worksheet.Cells[$"F{currentRow}"].Value = "Unit Price";
                worksheet.Cells[$"G{currentRow}"].Value = "Subtotal";
                worksheet.Cells[$"H{currentRow}"].Value = "Discount";
                worksheet.Cells[$"I{currentRow}"].Value = "VAT";
                worksheet.Cells[$"J{currentRow}"].Value = "Net Amount";

                // Style transaction headers
                using (var range = worksheet.Cells[$"A{currentRow}:J{currentRow}"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 11;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }
                worksheet.Row(currentRow).Height = 22;

                currentRow++;

                // Transaction Data with Items
                int rowCounter = 0;
                foreach (var transaction in report.Transactions)
                {
                    int transactionStartRow = currentRow;
                    bool isFirstItem = true;
                    bool isEvenRow = rowCounter % 2 == 0;

                    // If transaction has items, show them
                    if (transaction.Items != null && transaction.Items.Any())
                    {
                        foreach (var item in transaction.Items)
                        {
                            // Alternating row colors
                            Color rowColor = isEvenRow ? Color.White : Color.FromArgb(242, 242, 242);

                            // Invoice Number (only on first item)
                            if (isFirstItem)
                            {
                                worksheet.Cells[$"A{currentRow}"].Value = transaction.InvoiceNumber;
                                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            // Date & Time (only on first item)
                            if (isFirstItem)
                            {
                                worksheet.Cells[$"B{currentRow}"].Value = transaction.TransactionDate;
                                worksheet.Cells[$"B{currentRow}"].Style.Numberformat.Format = "mmm dd, yyyy hh:mm AM/PM";
                                worksheet.Cells[$"B{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[$"B{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);
                                worksheet.Cells[$"B{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[$"B{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            // Item Details with row color
                            worksheet.Cells[$"C{currentRow}"].Value = item.ProductName;
                            worksheet.Cells[$"C{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[$"C{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                            worksheet.Cells[$"D{currentRow}"].Value = item.UnitType;
                            worksheet.Cells[$"D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[$"D{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[$"D{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                            worksheet.Cells[$"E{currentRow}"].Value = item.Quantity;
                            worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[$"E{currentRow}"].Style.Font.Bold = true;
                            worksheet.Cells[$"E{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[$"E{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                            worksheet.Cells[$"F{currentRow}"].Value = item.UnitPrice;
                            worksheet.Cells[$"F{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                            worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheet.Cells[$"F{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[$"F{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                            worksheet.Cells[$"G{currentRow}"].Value = item.Subtotal;
                            worksheet.Cells[$"G{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                            worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            worksheet.Cells[$"G{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[$"G{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                            // Discount (only on first item)
                            if (isFirstItem)
                            {
                                // Get the discount details
                                var discount = await GetDiscountByIdAsync(transaction.DiscountId);

                                // Format discount display with name and percentage/amount
                                string discountText = "";
                                if (!string.IsNullOrEmpty(transaction.DiscountName) && transaction.DiscountAmount > 0)
                                {
                                    if (discount != null && discount.DiscountType?.ToLower() == "percentage")
                                    {
                                        // For percentage discounts, show the discount amount and percentage
                                        discountText = $"{transaction.DiscountName} ({discount.DiscountAmount:N2}%)\n₱{transaction.DiscountAmount:N2}";
                                    }
                                    else
                                    {
                                        // For fixed discounts, just show name and amount
                                        discountText = $"{transaction.DiscountName}\n₱{transaction.DiscountAmount:N2}";
                                    }
                                }
                                else if (transaction.DiscountAmount > 0)
                                {
                                    discountText = $"₱{transaction.DiscountAmount:N2}";
                                }
                                else
                                {
                                    discountText = "None";
                                }

                                worksheet.Cells[$"H{currentRow}"].Value = discountText;
                                worksheet.Cells[$"H{currentRow}"].Style.WrapText = true;
                                worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[$"H{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                worksheet.Cells[$"H{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[$"H{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));

                                if (transaction.DiscountAmount > 0)
                                {
                                    worksheet.Cells[$"H{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(192, 80, 77));
                                }
                            }

                            // VAT (only on first item)
                            if (isFirstItem)
                            {
                                string vatText = "";
                                if (transaction.VATRate.HasValue && transaction.VATAmount > 0)
                                {
                                    vatText = $"{transaction.VATRate:N2}%\n₱{transaction.VATAmount:N2}";
                                }
                                else if (transaction.VATAmount > 0)
                                {
                                    vatText = $"₱{transaction.VATAmount:N2}";
                                }
                                else
                                {
                                    vatText = "None";
                                }

                                worksheet.Cells[$"I{currentRow}"].Value = vatText;
                                worksheet.Cells[$"I{currentRow}"].Style.WrapText = true;
                                worksheet.Cells[$"I{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[$"I{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                worksheet.Cells[$"I{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[$"I{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                            }

                            // Net Amount (only on first item, merged vertically if multiple items)
                            if (isFirstItem)
                            {
                                worksheet.Cells[$"J{currentRow}"].Value = transaction.NetAmount;
                                worksheet.Cells[$"J{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                                worksheet.Cells[$"J{currentRow}"].Style.Font.Bold = true;
                                worksheet.Cells[$"J{currentRow}"].Style.Font.Size = 12;
                                worksheet.Cells[$"J{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                worksheet.Cells[$"J{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                worksheet.Cells[$"J{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[$"J{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));
                                worksheet.Cells[$"J{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 60));
                            }

                            // Add borders to each cell
                            worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            currentRow++;
                            isFirstItem = false;
                        }

                        // Merge cells for invoice and date if multiple items
                        if (transaction.Items.Count > 1)
                        {
                            worksheet.Cells[$"A{transactionStartRow}:A{currentRow - 1}"].Merge = true;
                            worksheet.Cells[$"A{transactionStartRow}:A{currentRow - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            worksheet.Cells[$"B{transactionStartRow}:B{currentRow - 1}"].Merge = true;
                            worksheet.Cells[$"B{transactionStartRow}:B{currentRow - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            worksheet.Cells[$"H{transactionStartRow}:H{currentRow - 1}"].Merge = true;
                            worksheet.Cells[$"H{transactionStartRow}:H{currentRow - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            worksheet.Cells[$"I{transactionStartRow}:I{currentRow - 1}"].Merge = true;
                            worksheet.Cells[$"I{transactionStartRow}:I{currentRow - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            worksheet.Cells[$"J{transactionStartRow}:J{currentRow - 1}"].Merge = true;
                            worksheet.Cells[$"J{transactionStartRow}:J{currentRow - 1}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                    }
                    else
                    {
                        // Transaction with no items (fallback)
                        Color rowColor = isEvenRow ? Color.White : Color.FromArgb(242, 242, 242);

                        worksheet.Cells[$"A{currentRow}"].Value = transaction.InvoiceNumber;
                        worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                        worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                        worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        worksheet.Cells[$"B{currentRow}"].Value = transaction.TransactionDate;
                        worksheet.Cells[$"B{currentRow}"].Style.Numberformat.Format = "mmm dd, yyyy hh:mm AM/PM";
                        worksheet.Cells[$"B{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[$"B{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);
                        worksheet.Cells[$"B{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"B{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        worksheet.Cells[$"C{currentRow}:G{currentRow}"].Merge = true;
                        worksheet.Cells[$"C{currentRow}"].Value = "No items recorded";
                        worksheet.Cells[$"C{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"C{currentRow}"].Style.Font.Italic = true;
                        worksheet.Cells[$"C{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[$"C{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                        // Discount
                        var discount = await GetDiscountByIdAsync(transaction.DiscountId);
                        string discountText = "";
                        if (!string.IsNullOrEmpty(transaction.DiscountName) && transaction.DiscountAmount > 0)
                        {
                            if (discount != null && discount.DiscountType?.ToLower() == "percentage")
                            {
                                discountText = $"{transaction.DiscountName} ({discount.DiscountAmount:N2}%)\n₱{transaction.DiscountAmount:N2}";
                            }
                            else
                            {
                                discountText = $"{transaction.DiscountName}\n₱{transaction.DiscountAmount:N2}";
                            }
                        }
                        else if (transaction.DiscountAmount > 0)
                        {
                            discountText = $"₱{transaction.DiscountAmount:N2}";
                        }
                        else
                        {
                            discountText = "None";
                        }

                        worksheet.Cells[$"H{currentRow}"].Value = discountText;
                        worksheet.Cells[$"H{currentRow}"].Style.WrapText = true;
                        worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"H{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[$"H{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[$"H{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));

                        if (transaction.DiscountAmount > 0)
                        {
                            worksheet.Cells[$"H{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(192, 80, 77));
                        }

                        // VAT
                        string vatText = "";
                        if (transaction.VATRate.HasValue && transaction.VATAmount > 0)
                        {
                            vatText = $"{transaction.VATRate:N2}%\n₱{transaction.VATAmount:N2}";
                        }
                        else if (transaction.VATAmount > 0)
                        {
                            vatText = $"₱{transaction.VATAmount:N2}";
                        }
                        else
                        {
                            vatText = "None";
                        }

                        worksheet.Cells[$"I{currentRow}"].Value = vatText;
                        worksheet.Cells[$"I{currentRow}"].Style.WrapText = true;
                        worksheet.Cells[$"I{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"I{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[$"I{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[$"I{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                        worksheet.Cells[$"J{currentRow}"].Value = transaction.NetAmount;
                        worksheet.Cells[$"J{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                        worksheet.Cells[$"J{currentRow}"].Style.Font.Bold = true;
                        worksheet.Cells[$"J{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        worksheet.Cells[$"J{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[$"J{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[$"J{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));
                        worksheet.Cells[$"J{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 60));

                        // Add borders
                        worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"A{currentRow}:J{currentRow}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        currentRow++;
                    }

                    rowCounter++;
                }

                // Add border around entire transaction section
                if (currentRow > transactionsStartRow + 2)
                {
                    using (var range = worksheet.Cells[transactionsStartRow, 1, currentRow - 1, 10])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                    }
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set minimum column widths
                worksheet.Column(1).Width = 15; // Invoice
                worksheet.Column(2).Width = 20; // Date
                worksheet.Column(3).Width = 30; // Product Name
                worksheet.Column(4).Width = 12; // Unit Type
                worksheet.Column(5).Width = 10; // Quantity
                worksheet.Column(6).Width = 13; // Unit Price
                worksheet.Column(7).Width = 13; // Subtotal
                worksheet.Column(8).Width = 18; // Discount
                worksheet.Column(9).Width = 15; // VAT
                worksheet.Column(10).Width = 16; // Net Amount

                return package.GetAsByteArray();
            }
        }

        //INVENTORY REPORTS
        public async Task<byte[]> ExportInventoryReportToExcelAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string filterType = "all",
            int? currentLoggedInUserId = null)
        {
            var report = await GetInventoryReportAsync(startDate, endDate, filterType);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Inventory Report");

                // Get the current logged-in user's name
                string adminName = "Administrator";
                if (currentLoggedInUserId.HasValue && currentLoggedInUserId.Value > 0)
                {
                    var currentUser = await GetEmployeeByIdAsync(currentLoggedInUserId.Value);
                    adminName = currentUser?.FullName ?? "Administrator";
                }

                string printDate = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

                // ===== HEADER SECTION =====
                int currentRow = 1;
                int headerStartRow = currentRow;

                // INVENTORY REPORT - Title (Centered)
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "INVENTORY REPORT";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 20;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 120));
                worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 35;

                currentRow++;

                // Pharmacy Name - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Racquel Pharmacy";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 25;

                currentRow++;

                // Pharmacy Address - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Q3H+HGC, Makabud, Novaliches, Caloocan, Metro Manila";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Empty row for spacing with background
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 5;

                currentRow++;

                // Period - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Period: " + (startDate.HasValue && endDate.HasValue
                    ? $"{startDate:MMM dd, yyyy} to {endDate:MMM dd, yyyy}"
                    : "All Time");
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Printed By - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Printed By: " + adminName;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Print Date - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Print Date: " + printDate;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                // Filter Type (if applicable)
                string filterDisplayName = filterType?.ToLower() switch
                {
                    "critical" or "critical stock" => "Critical Stock",
                    "near expiry" or "nearexpiry" => "Near Expiry",
                    "expired" => "Expired",
                    "all" or "all inventory" => "All Inventory",
                    _ => filterType ?? "All Inventory"
                };

                if (!string.IsNullOrEmpty(filterType) && filterType.ToLower() != "all" && filterType.ToLower() != "all inventory")
                {
                    currentRow++;
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                    worksheet.Cells[$"A{currentRow}"].Value = "Filter Type: " + filterDisplayName;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                    worksheet.Row(currentRow).Height = 20;
                }

                // Add border around entire header section
                using (var range = worksheet.Cells[headerStartRow, 1, currentRow, 15])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }

                currentRow += 2;

                // ===== SUMMARY SECTION =====
                int summaryStartRow = currentRow;

                // Summary Headers with styling
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Row(currentRow).Height = 25;

                // Merge cells for each summary metric
                worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Total Products";

                worksheet.Cells[$"E{currentRow}:H{currentRow}"].Merge = true;
                worksheet.Cells[$"E{currentRow}"].Value = "Total Value";

                worksheet.Cells[$"I{currentRow}:L{currentRow}"].Merge = true;
                worksheet.Cells[$"I{currentRow}"].Value = "Critical Stock Count";

                worksheet.Cells[$"M{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"M{currentRow}"].Value = "Near Expiry Count";

                currentRow++;

                // Summary Values
                worksheet.Cells[$"A{currentRow}:D{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = report.Summary.TotalProducts;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 30;

                worksheet.Cells[$"E{currentRow}:H{currentRow}"].Merge = true;
                worksheet.Cells[$"E{currentRow}"].Value = report.Summary.TotalValue;
                worksheet.Cells[$"E{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"E{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"E{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"E{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"E{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 60));
                worksheet.Cells[$"E{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"E{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));

                worksheet.Cells[$"I{currentRow}:L{currentRow}"].Merge = true;
                worksheet.Cells[$"I{currentRow}"].Value = report.Summary.CriticalStockCount;
                worksheet.Cells[$"I{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"I{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"I{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"I{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"I{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"I{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                if (report.Summary.CriticalStockCount > 0)
                {
                    worksheet.Cells[$"I{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(192, 80, 77));
                }

                worksheet.Cells[$"M{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"M{currentRow}"].Value = report.Summary.NearExpiryCount;
                worksheet.Cells[$"M{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"M{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"M{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"M{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"M{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"M{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                // Add border to summary section
                using (var range = worksheet.Cells[summaryStartRow, 1, currentRow, 15])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }

                // ===== INVENTORY DETAILS SECTION =====
                currentRow += 3;
                int detailsStartRow = currentRow;

                // Section Title
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "INVENTORY DETAILS";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 120));
                worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 25;

                currentRow++;

                // Detail Headers
                worksheet.Cells[$"A{currentRow}"].Value = "Product Name";
                worksheet.Cells[$"B{currentRow}"].Value = "Category";
                worksheet.Cells[$"C{currentRow}"].Value = "Brand";
                worksheet.Cells[$"D{currentRow}"].Value = "Unit Type";
                worksheet.Cells[$"E{currentRow}"].Value = "Batch Number";
                worksheet.Cells[$"F{currentRow}"].Value = "Box Qty";
                worksheet.Cells[$"G{currentRow}"].Value = "Sheet Qty";
                worksheet.Cells[$"H{currentRow}"].Value = "Piece Qty";
                worksheet.Cells[$"I{currentRow}"].Value = "Total Pieces";
                worksheet.Cells[$"J{currentRow}"].Value = "Reorder Level";
                worksheet.Cells[$"K{currentRow}"].Value = "Unit Price";
                worksheet.Cells[$"L{currentRow}"].Value = "Total Value";
                worksheet.Cells[$"M{currentRow}"].Value = "Mfg Date";
                worksheet.Cells[$"N{currentRow}"].Value = "Expiry Date";
                worksheet.Cells[$"O{currentRow}"].Value = "Status";

                // Style detail headers
                using (var range = worksheet.Cells[$"A{currentRow}:O{currentRow}"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 11;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }
                worksheet.Row(currentRow).Height = 22;

                currentRow++;

                // Detail Data with alternating row colors
                int rowCounter = 0;
                foreach (var inventory in report.InventoryItems)
                {
                    bool isEvenRow = rowCounter % 2 == 0;
                    Color rowColor = isEvenRow ? Color.White : Color.FromArgb(242, 242, 242);

                    // Product Name
                    worksheet.Cells[$"A{currentRow}"].Value = inventory.ProductName;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Category
                    worksheet.Cells[$"B{currentRow}"].Value = inventory.Product?.CategoryName ?? "N/A";
                    worksheet.Cells[$"B{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"B{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Brand
                    worksheet.Cells[$"C{currentRow}"].Value = inventory.Product?.BrandName ?? "N/A";
                    worksheet.Cells[$"C{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"C{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Unit Type
                    string unitType = "N/A";
                    if (inventory.Product?.UnitMeasurement != null)
                    {
                        unitType = inventory.Product.UnitMeasurement;
                    }
                    else if (inventory.SheetsPerBox.HasValue && inventory.SheetsPerBox > 0 &&
                        inventory.PiecesPerSheet.HasValue && inventory.PiecesPerSheet > 0)
                    {
                        unitType = "Multi-Level";
                    }
                    else if (inventory.PiecePerBox.HasValue && inventory.PiecePerBox > 0 &&
                        (!inventory.PiecesPerSheet.HasValue || inventory.PiecesPerSheet == 0))
                    {
                        unitType = "Dual-Level";
                    }
                    else if ((!inventory.SheetsPerBox.HasValue || inventory.SheetsPerBox == 0) &&
                        (!inventory.PiecePerBox.HasValue || inventory.PiecePerBox == 0))
                    {
                        unitType = "Single-Level";
                    }

                    worksheet.Cells[$"D{currentRow}"].Value = unitType;
                    worksheet.Cells[$"D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"D{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"D{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"D{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                    // Batch Number
                    worksheet.Cells[$"E{currentRow}"].Value = inventory.BatchNumber;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"E{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"E{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Box Quantity
                    worksheet.Cells[$"F{currentRow}"].Value = inventory.BoxQuantity;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"F{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"F{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Sheet Quantity
                    worksheet.Cells[$"G{currentRow}"].Value = inventory.SheetQuantity;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"G{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"G{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Piece Quantity
                    worksheet.Cells[$"H{currentRow}"].Value = inventory.PieceQuantity;
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"H{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"H{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Total Pieces Available
                    worksheet.Cells[$"I{currentRow}"].Value = inventory.TotalPiecesAvailable;
                    worksheet.Cells[$"I{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"I{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"I{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"I{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));

                    // Reorder Level
                    worksheet.Cells[$"J{currentRow}"].Value = inventory.ReorderLevel;
                    worksheet.Cells[$"J{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"J{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"J{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Unit Price
                    worksheet.Cells[$"K{currentRow}"].Value = inventory.PiecePrice;
                    worksheet.Cells[$"K{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                    worksheet.Cells[$"K{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"K{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"K{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Total Value
                    decimal totalValue = inventory.TotalPiecesAvailable * inventory.PiecePrice;
                    worksheet.Cells[$"L{currentRow}"].Value = totalValue;
                    worksheet.Cells[$"L{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                    worksheet.Cells[$"L{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"L{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"L{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"L{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));

                    // Manufacturing Date
                    worksheet.Cells[$"M{currentRow}"].Value = inventory.ManufacturingDate;
                    worksheet.Cells[$"M{currentRow}"].Style.Numberformat.Format = "mmm dd, yyyy";
                    worksheet.Cells[$"M{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"M{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"M{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Expiration Date
                    worksheet.Cells[$"N{currentRow}"].Value = inventory.ExpirationDate;
                    worksheet.Cells[$"N{currentRow}"].Style.Numberformat.Format = "mmm dd, yyyy";
                    worksheet.Cells[$"N{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"N{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"N{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // Status
                    string status = "Active";
                    Color statusColor = rowColor;
                    Color statusFontColor = Color.Black;

                    if (inventory.IsExpired)
                    {
                        status = "Expired";
                        statusColor = Color.FromArgb(255, 199, 206);
                        statusFontColor = Color.FromArgb(156, 0, 6);
                    }
                    else if (inventory.IsNearExpiry)
                    {
                        status = "Near Expiry";
                        statusColor = Color.FromArgb(255, 235, 156);
                        statusFontColor = Color.FromArgb(156, 101, 0);
                    }
                    else if (inventory.IsCriticalStock)
                    {
                        status = "Critical Stock";
                        statusColor = Color.FromArgb(255, 242, 204);
                        statusFontColor = Color.FromArgb(192, 80, 77);
                    }

                    worksheet.Cells[$"O{currentRow}"].Value = status;
                    worksheet.Cells[$"O{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"O{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"O{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"O{currentRow}"].Style.Fill.BackgroundColor.SetColor(statusColor);
                    worksheet.Cells[$"O{currentRow}"].Style.Font.Color.SetColor(statusFontColor);

                    // Add borders
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    currentRow++;
                    rowCounter++;
                }

                // Add border around entire details section
                if (currentRow > detailsStartRow + 2)
                {
                    using (var range = worksheet.Cells[detailsStartRow, 1, currentRow - 1, 15])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                    }
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set minimum column widths
                worksheet.Column(1).Width = 30;  // Product Name
                worksheet.Column(2).Width = 15;  // Category
                worksheet.Column(3).Width = 15;  // Brand
                worksheet.Column(4).Width = 14;  // Unit Type
                worksheet.Column(5).Width = 18;  // Batch Number
                worksheet.Column(6).Width = 10;  // Box Qty
                worksheet.Column(7).Width = 10;  // Sheet Qty
                worksheet.Column(8).Width = 10;  // Piece Qty
                worksheet.Column(9).Width = 12;  // Total Pieces
                worksheet.Column(10).Width = 13; // Reorder Level
                worksheet.Column(11).Width = 13; // Unit Price
                worksheet.Column(12).Width = 15; // Total Value
                worksheet.Column(13).Width = 15; // Mfg Date
                worksheet.Column(14).Width = 15; // Expiry Date
                worksheet.Column(15).Width = 16; // Status

                return package.GetAsByteArray();
            }
        }


        // FAST-MOVING PRODUCTS REPORT - COMPLETE EXPORT METHOD
        public async Task<byte[]> ExportFastMovingProductsToExcelAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int topN = 20,
            int? currentLoggedInUserId = null)
        {
            var report = await GetFastMovingProductsReportAsync(startDate, endDate, topN);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Fast-Moving Products");

                // Get the current logged-in user's name
                string adminName = "Administrator";
                if (currentLoggedInUserId.HasValue && currentLoggedInUserId.Value > 0)
                {
                    var currentUser = await GetEmployeeByIdAsync(currentLoggedInUserId.Value);
                    adminName = currentUser?.FullName ?? "Administrator";
                }

                string printDate = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");

                // ===== HEADER SECTION =====
                int currentRow = 1;
                int headerStartRow = currentRow;

                // FAST-MOVING PRODUCTS REPORT - Title (Centered)
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "FAST-MOVING PRODUCTS REPORT";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 20;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 120));
                worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 35;

                currentRow++;

                // Pharmacy Name - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Racquel Pharmacy";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 25;

                currentRow++;

                // Pharmacy Address - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Q3H+HGC, Makabud, Novaliches, Caloocan, Metro Manila";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Empty row for spacing with background
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 5;

                currentRow++;

                // Period - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Period: " + (startDate.HasValue && endDate.HasValue
                    ? $"{startDate:MMM dd, yyyy} to {endDate:MMM dd, yyyy}"
                    : "All Time");
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Printed By - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Printed By: " + adminName;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Print Date - Centered
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Print Date: " + printDate;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                worksheet.Row(currentRow).Height = 20;

                currentRow++;

                // Top Products Filter
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Top Products: Top " + topN;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 10;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 242, 204));
                worksheet.Row(currentRow).Height = 20;

                // Add border around entire header section
                using (var range = worksheet.Cells[headerStartRow, 1, currentRow, 15])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }

                currentRow += 2;

                // ===== SUMMARY SECTION =====
                int summaryStartRow = currentRow;

                // Summary Headers with styling
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Row(currentRow).Height = 25;

                // Merge cells for each summary metric
                worksheet.Cells[$"A{currentRow}:E{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "Total Products Sold";

                worksheet.Cells[$"F{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"F{currentRow}"].Value = "Total Quantity Sold";

                worksheet.Cells[$"K{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"K{currentRow}"].Value = "Total Revenue";

                currentRow++;

                // Summary Values
                worksheet.Cells[$"A{currentRow}:E{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = report.TotalProductsSold;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                worksheet.Row(currentRow).Height = 30;

                worksheet.Cells[$"F{currentRow}:J{currentRow}"].Merge = true;
                worksheet.Cells[$"F{currentRow}"].Value = report.TotalQuantitySold;
                worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"F{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"F{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"F{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"F{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"F{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                worksheet.Cells[$"K{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"K{currentRow}"].Value = report.TotalRevenue;
                worksheet.Cells[$"K{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                worksheet.Cells[$"K{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"K{currentRow}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[$"K{currentRow}"].Style.Font.Size = 16;
                worksheet.Cells[$"K{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"K{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 60));
                worksheet.Cells[$"K{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"K{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));

                // Add border to summary section
                using (var range = worksheet.Cells[summaryStartRow, 1, currentRow, 15])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }

                // ===== PRODUCT RANKINGS SECTION =====
                currentRow += 3;
                int detailsStartRow = currentRow;

                // Section Title
                worksheet.Cells[$"A{currentRow}:O{currentRow}"].Merge = true;
                worksheet.Cells[$"A{currentRow}"].Value = "PRODUCT RANKINGS";
                worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 14;
                worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 120));
                worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.White);
                worksheet.Row(currentRow).Height = 25;

                currentRow++;

                // Headers - 15 columns total (A to O)
                worksheet.Cells[$"A{currentRow}"].Value = "Rank";
                worksheet.Cells[$"B{currentRow}"].Value = "Product Name";
                worksheet.Cells[$"C{currentRow}"].Value = "Brand";
                worksheet.Cells[$"D{currentRow}"].Value = "Category";
                worksheet.Cells[$"E{currentRow}"].Value = "Generic";
                worksheet.Cells[$"F{currentRow}"].Value = "Classification";
                worksheet.Cells[$"G{currentRow}"].Value = "Unit Type";
                worksheet.Cells[$"H{currentRow}"].Value = "Box Sold";
                worksheet.Cells[$"I{currentRow}"].Value = "Sheet Sold";
                worksheet.Cells[$"J{currentRow}"].Value = "Piece Sold";
                worksheet.Cells[$"K{currentRow}"].Value = "Transaction Count";
                worksheet.Cells[$"L{currentRow}"].Value = "Revenue";
                worksheet.Cells[$"M{currentRow}"].Value = "Current Stock";
                worksheet.Cells[$"N{currentRow}"].Value = "Stock Status";
                worksheet.Cells[$"O{currentRow}"].Value = "Trend";

                using (var range = worksheet.Cells[$"A{currentRow}:O{currentRow}"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 11;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                }
                worksheet.Row(currentRow).Height = 22;

                currentRow++;

                // Get product details for unit types and quantities
                var productIds = report.Products.Select(p => p.ProductId).ToList();
                var products = await _getRepo.GetAllProductAsync();
                var productDict = products.Where(p => productIds.Contains(p.ProductID))
                    .ToDictionary(p => p.ProductID);

                int rowCounter = 0;
                foreach (var product in report.Products)
                {
                    bool isEvenRow = rowCounter % 2 == 0;
                    Color rowColor = isEvenRow ? Color.White : Color.FromArgb(242, 242, 242);

                    // Get product details
                    var productDetails = productDict.ContainsKey(product.ProductId)
                        ? productDict[product.ProductId]
                        : null;

                    // COLUMN A: Rank
                    worksheet.Cells[$"A{currentRow}"].Value = product.Rank;
                    worksheet.Cells[$"A{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"A{currentRow}"].Style.Font.Size = 12;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"A{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                    if (product.Rank == 1)
                        worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(255, 215, 0)); // Gold
                    else if (product.Rank == 2)
                        worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(192, 192, 192)); // Silver
                    else if (product.Rank == 3)
                        worksheet.Cells[$"A{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(205, 127, 50)); // Bronze

                    // COLUMN B: Product Name (with Formulation and Type)
                    string productNameText = product.ProductName;
                    if (productDetails != null)
                    {
                        var nameParts = new List<string> { productDetails.ProductName };

                        if (productDetails.Formulation != null && !string.IsNullOrEmpty(productDetails.Formulation.FormulationName))
                            nameParts.Add(productDetails.Formulation.FormulationName);

                        if (productDetails.TypeModel != null && !string.IsNullOrEmpty(productDetails.TypeModel.TypeName))
                            nameParts.Add(productDetails.TypeModel.TypeName);

                        productNameText = string.Join(" ", nameParts);
                    }

                    worksheet.Cells[$"B{currentRow}"].Value = productNameText;
                    worksheet.Cells[$"B{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"B{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);
                    worksheet.Cells[$"B{currentRow}"].Style.Font.Bold = true;

                    // COLUMN C: Brand
                    string brandName = productDetails?.Brand?.BrandName ?? "N/A";
                    worksheet.Cells[$"C{currentRow}"].Value = brandName;
                    worksheet.Cells[$"C{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"C{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"C{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                    // COLUMN D: Category
                    string categoryName = productDetails?.Category?.CategoryName ?? "N/A";
                    worksheet.Cells[$"D{currentRow}"].Value = categoryName;
                    worksheet.Cells[$"D{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"D{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"D{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                    // COLUMN E: Generic
                    string genericName = productDetails?.Generic?.GenericName ?? "N/A";
                    worksheet.Cells[$"E{currentRow}"].Value = genericName;
                    worksheet.Cells[$"E{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"E{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"E{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // COLUMN F: Classification
                    string classificationName = productDetails?.Classification?.ClassificationName ?? "N/A";
                    worksheet.Cells[$"F{currentRow}"].Value = classificationName;
                    worksheet.Cells[$"F{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"F{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"F{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // COLUMN G: Unit Type
                    string unitType = productDetails?.UnitMeasurement ?? "N/A";
                    worksheet.Cells[$"G{currentRow}"].Value = unitType;
                    worksheet.Cells[$"G{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"G{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"G{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"G{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));

                    // Calculate Box, Sheet, Piece quantities based on unit type
                    int boxSold = 0, sheetSold = 0, pieceSold = 0;

                    if (productDetails != null)
                    {
                        int totalPieces = product.QuantitySold;

                        if (unitType == "Multi-Level")
                        {
                            int sheetsPerBox = productDetails.SheetsPerBox;
                            int piecesPerSheet = productDetails.PiecesPerSheet;
                            int piecesPerBox = sheetsPerBox * piecesPerSheet;

                            if (piecesPerBox > 0)
                            {
                                boxSold = totalPieces / piecesPerBox;
                                int remainder = totalPieces % piecesPerBox;

                                if (piecesPerSheet > 0)
                                {
                                    sheetSold = remainder / piecesPerSheet;
                                    pieceSold = remainder % piecesPerSheet;
                                }
                            }
                        }
                        else if (unitType == "Dual-Level")
                        {
                            int piecesPerBox = productDetails.PiecePerBox;

                            if (piecesPerBox > 0)
                            {
                                boxSold = totalPieces / piecesPerBox;
                                pieceSold = totalPieces % piecesPerBox;
                            }
                        }
                        else
                        {
                            pieceSold = totalPieces;
                        }
                    }

                    // COLUMN H: Box Sold
                    worksheet.Cells[$"H{currentRow}"].Value = boxSold > 0 ? boxSold.ToString() : "-";
                    worksheet.Cells[$"H{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"H{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"H{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // COLUMN I: Sheet Sold
                    worksheet.Cells[$"I{currentRow}"].Value = sheetSold > 0 ? sheetSold.ToString() : "-";
                    worksheet.Cells[$"I{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"I{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"I{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // COLUMN J: Piece Sold
                    worksheet.Cells[$"J{currentRow}"].Value = pieceSold;
                    worksheet.Cells[$"J{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"J{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"J{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"J{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));

                    // COLUMN K: Transaction Count
                    worksheet.Cells[$"K{currentRow}"].Value = product.TransactionCount;
                    worksheet.Cells[$"K{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"K{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"K{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // COLUMN L: Revenue
                    worksheet.Cells[$"L{currentRow}"].Value = product.Revenue;
                    worksheet.Cells[$"L{currentRow}"].Style.Numberformat.Format = "₱#,##0.00";
                    worksheet.Cells[$"L{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[$"L{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"L{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"L{currentRow}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));
                    worksheet.Cells[$"L{currentRow}"].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 60));

                    // COLUMN M: Current Stock
                    worksheet.Cells[$"M{currentRow}"].Value = product.CurrentStock;
                    worksheet.Cells[$"M{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"M{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"M{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"M{currentRow}"].Style.Fill.BackgroundColor.SetColor(rowColor);

                    // COLUMN N: Stock Status
                    string stockStatus = "Adequate";
                    Color stockColor = Color.FromArgb(226, 239, 218);
                    Color stockFontColor = Color.FromArgb(0, 112, 60);

                    if (product.CurrentStock == 0)
                    {
                        stockStatus = "Out of Stock";
                        stockColor = Color.FromArgb(255, 199, 206);
                        stockFontColor = Color.FromArgb(156, 0, 6);
                    }
                    else if (product.CurrentStock < 10)
                    {
                        stockStatus = "Low Stock";
                        stockColor = Color.FromArgb(255, 242, 204);
                        stockFontColor = Color.FromArgb(192, 80, 77);
                    }
                    else if (product.CurrentStock < 50)
                    {
                        stockStatus = "Moderate";
                        stockColor = Color.FromArgb(255, 235, 156);
                        stockFontColor = Color.FromArgb(156, 101, 0);
                    }

                    worksheet.Cells[$"N{currentRow}"].Value = stockStatus;
                    worksheet.Cells[$"N{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"N{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"N{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"N{currentRow}"].Style.Fill.BackgroundColor.SetColor(stockColor);
                    worksheet.Cells[$"N{currentRow}"].Style.Font.Color.SetColor(stockFontColor);

                    // COLUMN O: Trend
                    Color trendColor = rowColor;
                    Color trendFontColor = Color.Black;

                    switch (product.Trend.ToLower())
                    {
                        case "increasing":
                            trendColor = Color.FromArgb(226, 239, 218);
                            trendFontColor = Color.FromArgb(0, 112, 60);
                            break;
                        case "stable":
                            trendColor = Color.FromArgb(217, 225, 242);
                            trendFontColor = Color.FromArgb(31, 78, 120);
                            break;
                        case "decreasing":
                            trendColor = Color.FromArgb(255, 199, 206);
                            trendFontColor = Color.FromArgb(156, 0, 6);
                            break;
                    }

                    worksheet.Cells[$"O{currentRow}"].Value = product.Trend;
                    worksheet.Cells[$"O{currentRow}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[$"O{currentRow}"].Style.Font.Bold = true;
                    worksheet.Cells[$"O{currentRow}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"O{currentRow}"].Style.Fill.BackgroundColor.SetColor(trendColor);
                    worksheet.Cells[$"O{currentRow}"].Style.Font.Color.SetColor(trendFontColor);

                    // Add borders
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[$"A{currentRow}:O{currentRow}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    worksheet.Row(currentRow).Height = 25;

                    currentRow++;
                    rowCounter++;
                }

                // Add border around entire details section
                if (currentRow > detailsStartRow + 2)
                {
                    using (var range = worksheet.Cells[detailsStartRow, 1, currentRow - 1, 15])
                    {
                        range.Style.Border.BorderAround(ExcelBorderStyle.Medium, Color.FromArgb(31, 78, 120));
                    }
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set column widths
                worksheet.Column(1).Width = 8;   // Rank
                worksheet.Column(2).Width = 30;  // Product Name
                worksheet.Column(3).Width = 15;  // Brand
                worksheet.Column(4).Width = 15;  // Category
                worksheet.Column(5).Width = 18;  // Generic
                worksheet.Column(6).Width = 18;  // Classification
                worksheet.Column(7).Width = 14;  // Unit Type
                worksheet.Column(8).Width = 12;  // Box Sold
                worksheet.Column(9).Width = 12;  // Sheet Sold
                worksheet.Column(10).Width = 12; // Piece Sold
                worksheet.Column(11).Width = 16; // Transaction Count
                worksheet.Column(12).Width = 15; // Revenue
                worksheet.Column(13).Width = 14; // Current Stock
                worksheet.Column(14).Width = 14; // Stock Status
                worksheet.Column(15).Width = 14; // Trend

                return package.GetAsByteArray();
            }
        }


        #endregion

        #region AdminDashboard
        public async Task<decimal> GetTodaySalesAmountAsync()
    {
      var todaySales = await _getRepo.GetTodaySalesAmountAsync();
      if (todaySales <= 0)
        return 0;
      return todaySales;
    }
    public async Task<int> GetInventoryLowStockCountAsync()
    {
      var lowStockCount = await _getRepo.GetInventoryLowStockCountAsync();
      if (lowStockCount == 0)
        return 0;
      return lowStockCount;
    }
    public async Task<int> GetInventoryOutOfStockCountAsync()
    {
      var outOfStockCount = await _getRepo.GetInventoryOutOfStockCountAsync();
      if (outOfStockCount == 0)
        return 0;
      return outOfStockCount;
    }
    public async Task<List<ProductSalesSummaryDto>> GetTopSellingProductsMonthlyAsync()
    {
      var topSelling = await _getRepo.GetTopSellingProductsMonthlyAsync();
      if (!topSelling.Any() || topSelling.Count() == 0)
        return new List<ProductSalesSummaryDto>();
      return topSelling;
    }
    #endregion

    #region BackUp Database
    public async Task<BackupListResponse> GetAllBackupsAsync()
    {
      try
      {
        return await _getRepo.GetAllBackupsAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error getting backups");
        return new BackupListResponse
        {
          Success = false,
          Backups = new List<BackupDto>(),
          TotalCount = 0,
          TotalSizeMB = "0.00 MB"
        };
      }
    }

    public async Task<byte[]?> DownloadBackupAsync(string fileName)
    {
      try
      {
        _logger.LogInformation($"Downloading backup: {fileName}");
        return await _getRepo.DownloadBackupAsync(fileName);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error downloading backup");
        return null;
      }
    }

    public async Task<BackupSettingsDto> GetBackupSettingsAsync()
    {
      return new BackupSettingsDto
      {
        AutoBackupEnabled = _config.GetValue<bool>("BackupSettings:AutoBackupEnabled", true),
        AutoBackupHour = _config.GetValue<int>("BackupSettings:AutoBackupHour", 2),       
        AutoBackupMinute = _config.GetValue<int>("BackupSettings:AutoBackupMinute", 0),    
        RetentionDays = _config.GetValue<int>("BackupSettings:RetentionDays", 30),
        MaxBackupCount = _config.GetValue<int>("BackupSettings:MaxBackupCount", 50)
      };
    }
    #endregion

    #region StockHistory
    public async Task<StockHistoryResponse> GetStockHistoryAsync(
        DateTime? startDate,
        DateTime? endDate,
        string? transactionType,
        string? searchTerm)
    {
      // Get filtered stock movements
      var movements = await _getRepo.GetStockHistoryAsync(
          startDate,
          endDate,
          transactionType,
          searchTerm);

      // Get summary data
      var summary = await _getRepo.GetStockHistorySummaryAsync(
          startDate,
          endDate,
          transactionType);

      // Map to DTOs
      var transactionDtos = movements.Select(sm => new StockHistoryDto
          {
          StockMovementID = sm.StockMovementID,
          MovementDate = sm.MovementDate,
          ProductName = sm.Inventory?.Product?.ProductName ?? string.Empty,
          BatchNumber = sm.Inventory?.BatchNumber ?? string.Empty,
          MovementType = sm.MovementType,
          ReferenceNumber = sm.ReferenceNumber,

          BoxQuantityBefore = sm.BoxQuantityBefore,
          SheetQuantityBefore = sm.SheetQuantityBefore,
          PieceQuantityBefore = sm.PieceQuantityBefore,

          BoxQuantityChanged = sm.BoxQuantityChanged,
          SheetQuantityChanged = sm.SheetQuantityChanged,
          PieceQuantityChanged = sm.PieceQuantityChanged,

          BoxQuantityAfter = sm.BoxQuantityAfter,
          SheetQuantityAfter = sm.SheetQuantityAfter,
          PieceQuantityAfter = sm.PieceQuantityAfter,

          EmployeeID = sm.EmployeeID,
          PerformedBy = sm.Employee != null ? $"{sm.Employee.FirstName} {sm.Employee.LastName}" : string.Empty,
          Notes = sm.Notes
          }).ToList();

      return new StockHistoryResponse
      {
        Summary = summary,
        Transactions = transactionDtos
      };
    }
    #endregion
  }
}

