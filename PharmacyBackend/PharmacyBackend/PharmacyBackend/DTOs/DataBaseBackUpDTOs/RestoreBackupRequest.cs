namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class RestoreBackupRequest
    {
        public string FileName { get; set; } = string.Empty;
        public bool ConfirmRestore { get; set; } // Safety check
    }
}
