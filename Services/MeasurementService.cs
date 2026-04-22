using ftpms.DTOs;
using ftpms.Interfaces;
using ftpms.Models;
using ftpms.ViewModels;

namespace ftpms.Services;

public class MeasurementService(IMeasurementRepository measurementRepository, ICustomerRepository customerRepository) : IMeasurementService
{
    public async Task<List<MeasurementDto>> GetAllAsync(int? customerId = null, string? templateType = null, CancellationToken cancellationToken = default)
    {
        var measurements = await measurementRepository.GetAllAsync(customerId, templateType, cancellationToken);
        return measurements.Select(MapToDto).ToList();
    }

    public async Task<List<MeasurementHistoryItemDto>> GetHistoryForCustomerAsync(int customerId, string? templateType = null, CancellationToken cancellationToken = default)
    {
        var measurements = await measurementRepository.GetByCustomerAsync(customerId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(templateType))
        {
            measurements = measurements.Where(x => x.TemplateType == templateType).ToList();
        }

        return measurements
            .OrderByDescending(m => m.DateTaken)
            .ThenByDescending(m => m.Version)
            .Select(m => new MeasurementHistoryItemDto
            {
                Id = m.Id,
                CustomerId = m.CustomerId,
                TemplateType = m.TemplateType,
                Version = m.Version,
                DateTaken = m.DateTaken,
                IsActive = m.IsActive,
                Notes = m.Notes
            })
            .ToList();
    }

    public async Task<MeasurementDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var measurement = await measurementRepository.GetByIdAsync(id, cancellationToken);
        return measurement is null ? null : MapToDto(measurement);
    }

    public async Task<MeasurementDto?> GetLatestAsync(int customerId, string templateType, CancellationToken cancellationToken = default)
    {
        var measurement = await measurementRepository.GetLatestAsync(customerId, templateType, cancellationToken);
        return measurement is null ? null : MapToDto(measurement);
    }

    public async Task<MeasurementPrefillDto> PreparePrefilledCreateModelAsync(int customerId, string templateType, CancellationToken cancellationToken = default)
    {
        var nextVersion = await measurementRepository.GetNextVersionAsync(customerId, templateType, cancellationToken);
        var latest = await measurementRepository.GetLatestAsync(customerId, templateType, cancellationToken);

        var model = latest is null
            ? new MeasurementInputViewModel
            {
                CustomerId = customerId,
                TemplateType = templateType,
                Version = nextVersion,
                DateTaken = DateTime.UtcNow.Date,
                IsActive = true
            }
            : MapToInput(latest);

        model.Version = nextVersion;
        model.DateTaken = DateTime.UtcNow.Date;
        model.ParentMeasurementId = latest?.Id;
        model.IsActive = true;

        return new MeasurementPrefillDto
        {
            Model = model,
            IsPrefilled = latest is not null,
            SourceMeasurementId = latest?.Id,
            SourceVersion = latest?.Version
        };
    }

    public async Task<(MeasurementDto? Created, Dictionary<string, List<string>> Errors)> CreateVersionedAsync(MeasurementInputViewModel input, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateCommonAsync(input, cancellationToken, requireVersion: false);
        MergeErrors(errors, ValidateByTemplate(input));

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var latest = await measurementRepository.GetLatestAsync(input.CustomerId, input.TemplateType, cancellationToken);
        input.Version = (latest?.Version ?? 0) + 1;
        input.ParentMeasurementId = latest?.Id;

        var measurement = MapToEntity(input);
        measurement.CreatedAtUtc = DateTime.UtcNow;
        measurement.IsActive = true;

        var created = await measurementRepository.CreateAsync(measurement, cancellationToken);
        var reloaded = await measurementRepository.GetByIdAsync(created.Id, cancellationToken) ?? created;

        return (MapToDto(reloaded), errors);
    }

    public async Task<(bool Updated, Dictionary<string, List<string>> Errors)> UpdateAsync(int id, MeasurementInputViewModel input, CancellationToken cancellationToken = default)
    {
        var existing = await measurementRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return (false, new Dictionary<string, List<string>> { ["Id"] = ["Measurement record not found."] });
        }

        var errors = await ValidateCommonAsync(input, cancellationToken, requireVersion: true);
        MergeErrors(errors, ValidateByTemplate(input));

        if (errors.Count > 0)
        {
            return (false, errors);
        }

        existing.CustomerId = input.CustomerId;
        existing.TemplateType = input.TemplateType;
        existing.Version = input.Version;
        existing.DateTaken = input.DateTaken;
        existing.Notes = input.Notes;
        existing.IsActive = input.IsActive;
        existing.ParentMeasurementId = input.ParentMeasurementId;

        existing.Chest = input.Chest;
        existing.Waist = input.Waist;
        existing.Hip = input.Hip;
        existing.Shoulder = input.Shoulder;
        existing.Neck = input.Neck;
        existing.SleeveLength = input.SleeveLength;
        existing.ArmRound = input.ArmRound;
        existing.Wrist = input.Wrist;
        existing.Bicep = input.Bicep;
        existing.TopLength = input.TopLength;
        existing.TrouserLength = input.TrouserLength;
        existing.Thigh = input.Thigh;
        existing.Knee = input.Knee;
        existing.Ankle = input.Ankle;
        existing.Inseam = input.Inseam;
        existing.BustPoint = input.BustPoint;
        existing.UnderBust = input.UnderBust;
        existing.RoundSleeve = input.RoundSleeve;
        existing.GownLength = input.GownLength;
        existing.SkirtLength = input.SkirtLength;

        existing.UpdatedAtUtc = DateTime.UtcNow;

        return (await measurementRepository.UpdateAsync(existing, cancellationToken), errors);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return measurementRepository.DeleteAsync(id, cancellationToken);
    }

    public Dictionary<string, List<string>> ValidateByTemplate(MeasurementInputViewModel input)
    {
        return MeasurementTemplateValidation.Validate(input);
    }

    private async Task<Dictionary<string, List<string>>> ValidateCommonAsync(MeasurementInputViewModel input, CancellationToken cancellationToken, bool requireVersion)
    {
        var errors = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        if (input.CustomerId <= 0)
        {
            errors["CustomerId"] = ["Customer is required."];
        }
        else
        {
            var customer = await customerRepository.GetByIdAsync(input.CustomerId, cancellationToken);
            if (customer is null)
            {
                errors["CustomerId"] = ["Selected customer was not found."];
            }
        }

        if (string.IsNullOrWhiteSpace(input.TemplateType))
        {
            errors["TemplateType"] = ["Template type is required."];
        }

        if (input.DateTaken == default)
        {
            errors["DateTaken"] = ["Date taken is required."];
        }

        if (requireVersion && input.Version <= 0)
        {
            errors["Version"] = ["Version is required."];
        }

        return errors;
    }

    private static void MergeErrors(IDictionary<string, List<string>> destination, Dictionary<string, List<string>> source)
    {
        foreach (var (key, messages) in source)
        {
            if (!destination.TryGetValue(key, out var existingMessages))
            {
                destination[key] = messages;
                continue;
            }

            existingMessages.AddRange(messages);
        }
    }

    private static Measurement MapToEntity(MeasurementInputViewModel input)
    {
        return new Measurement
        {
            CustomerId = input.CustomerId,
            TemplateType = input.TemplateType,
            Version = input.Version,
            DateTaken = input.DateTaken,
            Notes = input.Notes,
            IsActive = input.IsActive,
            ParentMeasurementId = input.ParentMeasurementId,
            Chest = input.Chest,
            Waist = input.Waist,
            Hip = input.Hip,
            Shoulder = input.Shoulder,
            Neck = input.Neck,
            SleeveLength = input.SleeveLength,
            ArmRound = input.ArmRound,
            Wrist = input.Wrist,
            Bicep = input.Bicep,
            TopLength = input.TopLength,
            TrouserLength = input.TrouserLength,
            Thigh = input.Thigh,
            Knee = input.Knee,
            Ankle = input.Ankle,
            Inseam = input.Inseam,
            BustPoint = input.BustPoint,
            UnderBust = input.UnderBust,
            RoundSleeve = input.RoundSleeve,
            GownLength = input.GownLength,
            SkirtLength = input.SkirtLength
        };
    }

    private static MeasurementInputViewModel MapToInput(Measurement measurement)
    {
        return new MeasurementInputViewModel
        {
            CustomerId = measurement.CustomerId,
            TemplateType = measurement.TemplateType,
            Version = measurement.Version,
            DateTaken = measurement.DateTaken,
            Notes = measurement.Notes,
            IsActive = measurement.IsActive,
            ParentMeasurementId = measurement.ParentMeasurementId,
            Chest = measurement.Chest,
            Waist = measurement.Waist,
            Hip = measurement.Hip,
            Shoulder = measurement.Shoulder,
            Neck = measurement.Neck,
            SleeveLength = measurement.SleeveLength,
            ArmRound = measurement.ArmRound,
            Wrist = measurement.Wrist,
            Bicep = measurement.Bicep,
            TopLength = measurement.TopLength,
            TrouserLength = measurement.TrouserLength,
            Thigh = measurement.Thigh,
            Knee = measurement.Knee,
            Ankle = measurement.Ankle,
            Inseam = measurement.Inseam,
            BustPoint = measurement.BustPoint,
            UnderBust = measurement.UnderBust,
            RoundSleeve = measurement.RoundSleeve,
            GownLength = measurement.GownLength,
            SkirtLength = measurement.SkirtLength
        };
    }

    private static MeasurementDto MapToDto(Measurement measurement)
    {
        return new MeasurementDto
        {
            Id = measurement.Id,
            CustomerId = measurement.CustomerId,
            CustomerName = measurement.Customer is null
                ? string.Empty
                : $"{measurement.Customer.FirstName} {measurement.Customer.LastName}".Trim(),
            TemplateType = measurement.TemplateType,
            Version = measurement.Version,
            DateTaken = measurement.DateTaken,
            Notes = measurement.Notes,
            IsActive = measurement.IsActive,
            ParentMeasurementId = measurement.ParentMeasurementId,
            Chest = measurement.Chest,
            Waist = measurement.Waist,
            Hip = measurement.Hip,
            Shoulder = measurement.Shoulder,
            Neck = measurement.Neck,
            SleeveLength = measurement.SleeveLength,
            ArmRound = measurement.ArmRound,
            Wrist = measurement.Wrist,
            Bicep = measurement.Bicep,
            TopLength = measurement.TopLength,
            TrouserLength = measurement.TrouserLength,
            Thigh = measurement.Thigh,
            Knee = measurement.Knee,
            Ankle = measurement.Ankle,
            Inseam = measurement.Inseam,
            BustPoint = measurement.BustPoint,
            UnderBust = measurement.UnderBust,
            RoundSleeve = measurement.RoundSleeve,
            GownLength = measurement.GownLength,
            SkirtLength = measurement.SkirtLength
        };
    }
}
