namespace ftpms.DTOs;

public class MeasurementDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string TemplateType { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTime DateTaken { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public int? ParentMeasurementId { get; set; }

    public decimal? Chest { get; set; }
    public decimal? Waist { get; set; }
    public decimal? Hip { get; set; }
    public decimal? Shoulder { get; set; }
    public decimal? Neck { get; set; }
    public decimal? SleeveLength { get; set; }
    public decimal? ArmRound { get; set; }
    public decimal? Wrist { get; set; }
    public decimal? Bicep { get; set; }
    public decimal? TopLength { get; set; }
    public decimal? TrouserLength { get; set; }
    public decimal? Thigh { get; set; }
    public decimal? Knee { get; set; }
    public decimal? Ankle { get; set; }
    public decimal? Inseam { get; set; }
    public decimal? BustPoint { get; set; }
    public decimal? UnderBust { get; set; }
    public decimal? RoundSleeve { get; set; }
    public decimal? GownLength { get; set; }
    public decimal? SkirtLength { get; set; }
}
