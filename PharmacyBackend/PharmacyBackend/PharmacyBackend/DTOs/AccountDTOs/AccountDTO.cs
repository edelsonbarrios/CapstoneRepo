namespace PharmacyBackend.DTOs.AccountDTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public int AccountCode { get; set; }
        public string Username { get; set; }
        public string DecryptedPassword { get; set; }
        public int EmployeeId { get; set; }
    }
}
