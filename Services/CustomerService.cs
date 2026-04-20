using ftpms.DTOs;
using ftpms.Interfaces;
using ftpms.Models;
using ftpms.ViewModels;

namespace ftpms.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await customerRepository.GetAllAsync(cancellationToken);
        return customers.Select(MapToDto).ToList();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id, cancellationToken);
        return customer is null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CustomerInputViewModel input, CancellationToken cancellationToken = default)
    {
        var customer = new Customer
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            Address = input.Address,
            CreatedAtUtc = DateTime.UtcNow
        };

        var created = await customerRepository.CreateAsync(customer, cancellationToken);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, CustomerInputViewModel input, CancellationToken cancellationToken = default)
    {
        var existing = await customerRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        existing.FirstName = input.FirstName;
        existing.LastName = input.LastName;
        existing.Email = input.Email;
        existing.PhoneNumber = input.PhoneNumber;
        existing.Address = input.Address;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        return await customerRepository.UpdateAsync(existing, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return customerRepository.DeleteAsync(id, cancellationToken);
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address
        };
    }
}
