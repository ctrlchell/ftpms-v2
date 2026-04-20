namespace ftpms.Models;

public class Invoice : BaseEntity
{
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public DateTime IssuedOnUtc { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Draft";
}
