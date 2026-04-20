using ftpms.Data;
using ftpms.Interfaces;
using ftpms.Models;
using Microsoft.EntityFrameworkCore;

namespace ftpms.Repositories;

public class CustomerRepository(ApplicationDbContext dbContext) : ICustomerRepository
{
    public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return customer;
    }

    public async Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Customers.AnyAsync(c => c.Id == customer.Id, cancellationToken);
        if (!exists)
        {
            return false;
        }

        dbContext.Customers.Update(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
