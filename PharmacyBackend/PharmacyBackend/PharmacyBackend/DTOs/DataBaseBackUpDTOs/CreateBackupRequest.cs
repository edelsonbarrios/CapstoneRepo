namespace PharmacyBackend.DTOs.DataBaseBackUpDTOs
{
    public class CreateBackupRequest
    {
        public string? Description { get; set; }
        public bool IsManual { get; set; } = true;
    }
}
