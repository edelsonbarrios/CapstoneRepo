namespace PharmacyBackend.DTOs.EmployeeHistoryDTO
{
    public class EmployeeHistoryDTO
    {
        public string EmployeeChanged { get; set; }
        public string ActionPerformed { get; set; }
        public string PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
    }
}
