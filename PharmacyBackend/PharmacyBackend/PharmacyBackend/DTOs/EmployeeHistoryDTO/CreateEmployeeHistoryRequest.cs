namespace PharmacyBackend.DTOs.EmployeeHistoryDTO
{
    public class CreateEmployeeHistoryRequest
    {
        public int EmployeeId { get; set; }
        public string ActionPerformed { get; set; } = "Created";
        public int PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    }
}
