using ftpms.DTOs;
using ftpms.ViewModels;

namespace ftpms.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CustomerDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CustomerDto> CreateAsync(CustomerInputViewModel input, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, CustomerInputViewModel input, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
