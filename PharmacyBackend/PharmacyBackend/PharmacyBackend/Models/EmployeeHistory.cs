namespace PharmacyBackend.Models
{
    public class EmployeeHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ActionPerformed { get; set; }
        public int PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        public Employee Employee { get; set; }
        public Employee PerformedByEmployee { get; set; }
    }
}
