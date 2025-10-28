namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class RestoreBackupRequest
    {
        public string fileName { get; set; } = string.Empty;
        public bool ConfirmRestore { get; set; } // Safety check
    }
}
