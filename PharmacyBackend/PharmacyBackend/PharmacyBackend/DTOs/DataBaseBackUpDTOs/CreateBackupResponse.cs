namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class CreateBackupResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BackupDto? Backup { get; set; }
    }
}
