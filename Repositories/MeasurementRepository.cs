using ftpms.Data;
using ftpms.Interfaces;
using ftpms.Models;
using Microsoft.EntityFrameworkCore;

namespace ftpms.Repositories;

public class MeasurementRepository(ApplicationDbContext dbContext) : IMeasurementRepository
{
    public async Task<List<Measurement>> GetAllAsync(int? customerId = null, string? templateType = null, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Measurements
            .Include(m => m.Customer)
            .AsNoTracking()
            .AsQueryable();

        if (customerId.HasValue)
        {
            query = query.Where(m => m.CustomerId == customerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(templateType))
        {
            query = query.Where(m => m.TemplateType == templateType);
        }

        return await query
            .OrderByDescending(m => m.DateTaken)
            .ThenByDescending(m => m.Version)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Measurement>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Measurements
            .Include(m => m.Customer)
            .AsNoTracking()
            .Where(m => m.CustomerId == customerId)
            .OrderByDescending(m => m.DateTaken)
            .ThenByDescending(m => m.Version)
            .ToListAsync(cancellationToken);
    }

    public Task<Measurement?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return dbContext.Measurements
            .Include(m => m.Customer)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public Task<Measurement?> GetLatestAsync(int customerId, string templateType, CancellationToken cancellationToken = default)
    {
        return dbContext.Measurements
            .AsNoTracking()
            .Where(m => m.CustomerId == customerId && m.TemplateType == templateType)
            .OrderByDescending(m => m.DateTaken)
            .ThenByDescending(m => m.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetNextVersionAsync(int customerId, string templateType, CancellationToken cancellationToken = default)
    {
        var latestVersion = await dbContext.Measurements
            .Where(m => m.CustomerId == customerId && m.TemplateType == templateType)
            .Select(m => (int?)m.Version)
            .MaxAsync(cancellationToken);

        return (latestVersion ?? 0) + 1;
    }

    public async Task<Measurement> CreateAsync(Measurement measurement, CancellationToken cancellationToken = default)
    {
        dbContext.Measurements.Add(measurement);
        await dbContext.SaveChangesAsync(cancellationToken);
        return measurement;
    }

    public async Task<bool> UpdateAsync(Measurement measurement, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Measurements.AnyAsync(m => m.Id == measurement.Id, cancellationToken);
        if (!exists)
        {
            return false;
        }

        dbContext.Measurements.Update(measurement);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var measurement = await dbContext.Measurements.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (measurement is null)
        {
            return false;
        }

        dbContext.Measurements.Remove(measurement);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
