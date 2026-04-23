using ftpms.ViewModels;

namespace ftpms.DTOs;

public class MeasurementPrefillDto
{
    public required MeasurementInputViewModel Model { get; set; }
    public bool IsPrefilled { get; set; }
    public int? SourceMeasurementId { get; set; }
    public int? SourceVersion { get; set; }
}
