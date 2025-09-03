namespace PharmacyBackend.DTOs.EmployeeHistoryDTO
{
    public class ArchiveEmployeeHistoryRequest
    {
        public int EmployeeId { get; set; }
        public string ActionPerformed { get; set; } = "Archived";
        public int PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    }
}
