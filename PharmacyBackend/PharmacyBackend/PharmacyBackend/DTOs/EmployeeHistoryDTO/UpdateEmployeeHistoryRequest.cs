namespace PharmacyBackend.DTOs.EmployeeHistoryDTO
{
    public class UpdateEmployeeHistoryRequest
    {
        public int EmployeeId { get; set; }
        public string ActionPerformed { get; set; } = "Update";
        public int PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    }
}
