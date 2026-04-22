using ftpms.Models;

namespace ftpms.Interfaces;

public interface IMeasurementRepository
{
    Task<List<Measurement>> GetAllAsync(int? customerId = null, string? templateType = null, CancellationToken cancellationToken = default);
    Task<List<Measurement>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    Task<Measurement?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Measurement?> GetLatestAsync(int customerId, string templateType, CancellationToken cancellationToken = default);
    Task<int> GetNextVersionAsync(int customerId, string templateType, CancellationToken cancellationToken = default);
    Task<Measurement> CreateAsync(Measurement measurement, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Measurement measurement, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
