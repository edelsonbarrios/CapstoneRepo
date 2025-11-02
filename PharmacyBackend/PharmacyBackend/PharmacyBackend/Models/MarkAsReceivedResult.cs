namespace PharmacyBackend.Models 
{
  public class MarkAsReceivedResult 
  {
    public bool Success {get; set;}
    public string ProductName {get; set;}
    public string Message { get; set;}
  }
}
