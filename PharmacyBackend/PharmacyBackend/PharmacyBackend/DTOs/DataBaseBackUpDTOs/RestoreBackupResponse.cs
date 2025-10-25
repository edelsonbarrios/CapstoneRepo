namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class RestoreBackupResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? RestoredDate { get; set; }
    }
}
