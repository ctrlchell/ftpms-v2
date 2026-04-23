using System.ComponentModel.DataAnnotations;

namespace ftpms.ViewModels;

public class MeasurementInputViewModel
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(50)]
    public string TemplateType { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Version { get; set; }

    [Required]
    public DateTime DateTaken { get; set; } = DateTime.UtcNow.Date;

    [StringLength(1000)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
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
