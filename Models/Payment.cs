namespace ftpms.Models;

public class Payment : BaseEntity
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidOnUtc { get; set; } = DateTime.UtcNow;
    public string Method { get; set; } = string.Empty;
}
