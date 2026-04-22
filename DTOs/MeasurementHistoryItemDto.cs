namespace ftpms.DTOs;

public class MeasurementHistoryItemDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string TemplateType { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTime DateTaken { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
}
