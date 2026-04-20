namespace ftpms.Models;

public class Measurement : BaseEntity
{
    public int CustomerId { get; set; }
    public string MeasurementDataJson { get; set; } = string.Empty;
}
