using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PharmacyBackend.Models.CashierViewModel;

namespace PharmacyBackend.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string LastName { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public string CivilStatus { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }

        public string FullName
        {
            get { return $"{LastName}, {FirstName}"; }
        }

        [JsonIgnore]
        public Role Role { get; set; } 
        [JsonIgnore]
        public Account Account { get; set; }

        //for navigation

        [JsonIgnore]
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        [JsonIgnore]
        public ICollection<SalesTransaction> SalesTransactions { get; set; } = new List<SalesTransaction>();
        [JsonIgnore]
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
