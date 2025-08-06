namespace PharmacyBackend.DTOs.AccountDTOs
{
    public class CreateAccountRequest
    {
        public string AccountCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
