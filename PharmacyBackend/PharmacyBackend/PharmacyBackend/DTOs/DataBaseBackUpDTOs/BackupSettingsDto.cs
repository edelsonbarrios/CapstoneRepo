namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class BackupSettingsDto
    {
        public bool AutoBackupEnabled { get; set; }
        public TimeSpan AutoBackupTime { get; set; } // Time of day for automatic backup
        public int RetentionDays { get; set; } // How many days to keep backups
        public int MaxBackupCount { get; set; } // Maximum number of backups to keep
    }
}
