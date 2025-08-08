namespace PharmacyBackend.DTOs.AccountDTOs
{
    public class UpdateAccountRequest
    {
        public int Id { get; set; }
        public string AccountCode { get; set; }
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }
    }
}
