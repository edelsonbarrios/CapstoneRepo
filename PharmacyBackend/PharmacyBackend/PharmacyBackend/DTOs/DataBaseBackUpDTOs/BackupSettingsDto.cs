using System.ComponentModel.DataAnnotations;

namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class BackupSettingsDto
    {
        public bool AutoBackupEnabled { get; set; }

        [Range(0, 23, ErrorMessage = "Hour must be between 0 and 23")]
        public int AutoBackupHour { get; set; }

        [Range(0, 59, ErrorMessage = "Minute must be between 0 and 59")]
        public int AutoBackupMinute { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Retention days must be at least 1")]
        public int RetentionDays { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Max backup count must be at least 1")]
        public int MaxBackupCount { get; set; }

        // Helper property for backward compatibility
        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan AutoBackupTime => new TimeSpan(AutoBackupHour, AutoBackupMinute, 0);
    }
}
   
