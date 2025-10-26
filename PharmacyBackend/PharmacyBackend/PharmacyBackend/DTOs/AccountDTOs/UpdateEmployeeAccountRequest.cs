namespace PharmacyBackend.DTOs.AccountDTOs
{
    public class UpdateEmployeeAccountRequest
    {
        public string CurrentPassword { get; set; }
        public string NewUserName { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public int AccountId { get; set; }
    }
}
