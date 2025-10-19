using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PharmacyBackend.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }
        public int EmployeeId { get; set; }
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public Employee Employee { get; set; }
    }
}
