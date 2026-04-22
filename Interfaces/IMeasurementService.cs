using ftpms.DTOs;
using ftpms.ViewModels;

namespace ftpms.Interfaces;

public interface IMeasurementService
{
    Task<List<MeasurementDto>> GetAllAsync(int? customerId = null, string? templateType = null, CancellationToken cancellationToken = default);
    Task<List<MeasurementHistoryItemDto>> GetHistoryForCustomerAsync(int customerId, string? templateType = null, CancellationToken cancellationToken = default);
    Task<MeasurementDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MeasurementDto?> GetLatestAsync(int customerId, string templateType, CancellationToken cancellationToken = default);
    Task<MeasurementPrefillDto> PreparePrefilledCreateModelAsync(int customerId, string templateType, CancellationToken cancellationToken = default);
    Task<(MeasurementDto? Created, Dictionary<string, List<string>> Errors)> CreateVersionedAsync(MeasurementInputViewModel input, CancellationToken cancellationToken = default);
    Task<(bool Updated, Dictionary<string, List<string>> Errors)> UpdateAsync(int id, MeasurementInputViewModel input, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Dictionary<string, List<string>> ValidateByTemplate(MeasurementInputViewModel input);
}
