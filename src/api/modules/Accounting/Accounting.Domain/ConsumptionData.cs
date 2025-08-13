using Accounting.Domain.Events.ConsumptionData;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class ConsumptionData : AuditableEntity, IAggregateRoot
{
    public DefaultIdType MeterId { get; private set; }
    public DateTime ReadingDate { get; private set; }
    public decimal CurrentReading { get; private set; }
    public decimal PreviousReading { get; private set; }
    public decimal KWhUsed { get; private set; }
    public string BillingPeriod { get; private set; }
    public string ReadingType { get; private set; } // "Actual", "Estimated", "Customer Read"
    public decimal? Multiplier { get; private set; } // For CT/PT ratios
    public bool IsValidReading { get; private set; }
    public string? ReadingSource { get; private set; } // "AMR", "Manual", "AMI"
    
    private ConsumptionData()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private ConsumptionData(DefaultIdType meterId, DateTime readingDate,
        decimal currentReading, decimal previousReading, string billingPeriod,
        string readingType = "Actual", decimal? multiplier = null, string? readingSource = null,
        string? description = null, string? notes = null)
    {
        MeterId = meterId;
        ReadingDate = readingDate;
        CurrentReading = currentReading;
        PreviousReading = previousReading;
        KWhUsed = CalculateUsage(currentReading, previousReading, multiplier);
        BillingPeriod = billingPeriod.Trim();
        ReadingType = readingType.Trim();
        Multiplier = multiplier ?? 1;
        IsValidReading = ValidateReading(currentReading, previousReading);
        ReadingSource = readingSource?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new ConsumptionDataCreated(Id, MeterId, ReadingDate, KWhUsed, BillingPeriod, Description, Notes));
    }

    public static ConsumptionData Create(DefaultIdType meterId, DateTime readingDate,
        decimal currentReading, decimal previousReading, string billingPeriod,
        string readingType = "Actual", decimal? multiplier = null, string? readingSource = null,
        string? description = null, string? notes = null)
    {
        if (currentReading < 0 || previousReading < 0)
            throw new InvalidMeterReadingException();

        return new ConsumptionData(meterId, readingDate, currentReading,
            previousReading, billingPeriod, readingType, multiplier, readingSource, description, notes);
    }

    public ConsumptionData Update(decimal? currentReading, decimal? previousReading,
        string? readingType, decimal? multiplier, string? readingSource,
        string? description, string? notes)
    {
        bool isUpdated = false;

        if (currentReading.HasValue && CurrentReading != currentReading.Value)
        {
            if (currentReading.Value < 0)
                throw new InvalidMeterReadingException();
            CurrentReading = currentReading.Value;
            KWhUsed = CalculateUsage(CurrentReading, PreviousReading, Multiplier);
            IsValidReading = ValidateReading(CurrentReading, PreviousReading);
            isUpdated = true;
        }

        if (previousReading.HasValue && PreviousReading != previousReading.Value)
        {
            if (previousReading.Value < 0)
                throw new InvalidMeterReadingException();
            PreviousReading = previousReading.Value;
            KWhUsed = CalculateUsage(CurrentReading, PreviousReading, Multiplier);
            IsValidReading = ValidateReading(CurrentReading, PreviousReading);
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(readingType) && ReadingType != readingType)
        {
            ReadingType = readingType.Trim();
            isUpdated = true;
        }

        if (multiplier.HasValue && Multiplier != multiplier.Value)
        {
            Multiplier = multiplier.Value;
            KWhUsed = CalculateUsage(CurrentReading, PreviousReading, Multiplier);
            isUpdated = true;
        }

        if (readingSource != ReadingSource)
        {
            ReadingSource = readingSource?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ConsumptionDataUpdated(this));
        }

        return this;
    }

    private static decimal CalculateUsage(decimal current, decimal previous, decimal? multiplier)
    {
        var usage = current - previous;
        return usage * (multiplier ?? 1);
    }

    private static bool ValidateReading(decimal current, decimal previous)
    {
        // Basic validation - current should be >= previous for most meters
        return current >= previous;
    }

    public ConsumptionData MarkAsEstimated(string reason)
    {
        ReadingType = "Estimated";
        Notes = $"{Notes}; Estimated: {reason}".Trim(';', ' ');
        QueueDomainEvent(new ConsumptionDataMarkedAsEstimated(Id, MeterId, reason));
        return this;
    }
}
