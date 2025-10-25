namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class BackupDto
    {
        public string FileName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileSizeMB => $"{FileSizeBytes / (1024.0 * 1024.0):F2} MB";
        public string BackupType { get; set; } = string.Empty; // Manual or Automated
        public string Status { get; set; } = string.Empty; // Success, Failed, In Progress
    }
}
