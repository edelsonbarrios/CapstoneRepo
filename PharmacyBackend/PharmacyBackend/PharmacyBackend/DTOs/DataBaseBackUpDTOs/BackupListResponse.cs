namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class BackupListResponse
    {
        public bool Success { get; set; }
        public List<BackupDto> Backups { get; set; } = new();
        public int TotalCount { get; set; }
        public string TotalSizeMB { get; set; } = string.Empty;
    }
}
