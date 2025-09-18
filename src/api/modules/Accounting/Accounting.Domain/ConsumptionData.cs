using Accounting.Domain.Events.ConsumptionData;

namespace Accounting.Domain;

/// <summary>
/// Records a meter's consumption snapshot for a billing period, including readings, usage, and validation metadata.
/// </summary>
/// <remarks>
/// Calculates <see cref="KWhUsed"/> from current/previous readings and multiplier and flags <see cref="IsValidReading"/>.
/// Defaults: <see cref="Multiplier"/> defaults to 1 when null or non-positive; strings are trimmed and length-limited.
/// </remarks>
public class ConsumptionData : AuditableEntity, IAggregateRoot
{
    private const int MaxBillingPeriodLength = 64;
    private const int MaxReadingTypeLength = 32;
    private const int MaxReadingSourceLength = 32;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Identifier of the meter this reading belongs to.
    /// </summary>
    public DefaultIdType MeterId { get; private set; }

    /// <summary>
    /// When the reading was taken.
    /// </summary>
    public DateTime ReadingDate { get; private set; }

    /// <summary>
    /// The new (current) register reading.
    /// </summary>
    public decimal CurrentReading { get; private set; }

    /// <summary>
    /// The prior register reading used to compute consumption.
    /// </summary>
    public decimal PreviousReading { get; private set; }

    /// <summary>
    /// Calculated usage in kWh for the period: (Current - Previous) * Multiplier.
    /// </summary>
    public decimal KWhUsed { get; private set; }

    /// <summary>
    /// Human-friendly period label (e.g., "2025-08"). Trimmed and capped in length.
    /// </summary>
    public string BillingPeriod { get; private set; } = string.Empty;

    /// <summary>
    /// Reading classification: "Actual", "Estimated", or "Customer Read".
    /// </summary>
    public string ReadingType { get; private set; } = string.Empty; // "Actual", "Estimated", "Customer Read"

    /// <summary>
    /// Optional ratio for CT/PT meters; defaults to 1 if not provided or invalid.
    /// </summary>
    public decimal? Multiplier { get; private set; } // For CT/PT ratios

    /// <summary>
    /// Flag indicating whether the reading sequence appears valid (current >= previous).
    /// </summary>
    public bool IsValidReading { get; private set; }

    /// <summary>
    /// Source of the reading: "AMR", "Manual", or "AMI".
    /// </summary>
    public string? ReadingSource { get; private set; } // "AMR", "Manual", "AMI"

    // Parameterless constructor for EF Core
    private ConsumptionData()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private ConsumptionData(DefaultIdType meterId, DateTime readingDate,
        decimal currentReading, decimal previousReading, string billingPeriod,
        string readingType = "Actual", decimal? multiplier = null, string? readingSource = null,
        string? description = null, string? notes = null)
    {
        if (meterId == default)
            throw new ArgumentException("MeterId is required.");

        if (currentReading < 0 || previousReading < 0)
            throw new InvalidMeterReadingException();

        MeterId = meterId;
        ReadingDate = readingDate;
        CurrentReading = currentReading;
        PreviousReading = previousReading;

        Multiplier = (multiplier.HasValue && multiplier.Value > 0) ? multiplier.Value : 1m;
        KWhUsed = CalculateUsage(CurrentReading, PreviousReading, Multiplier);

        var bp = (billingPeriod ?? string.Empty).Trim();
        BillingPeriod = bp.Length > MaxBillingPeriodLength ? bp.Substring(0, MaxBillingPeriodLength) : bp;

        var rt = (readingType ?? "Actual").Trim();
        ReadingType = rt.Length > MaxReadingTypeLength ? rt.Substring(0, MaxReadingTypeLength) : rt;

        IsValidReading = ValidateReading(CurrentReading, PreviousReading);

        ReadingSource = (readingSource ?? string.Empty).Trim();
        if (ReadingSource?.Length == 0) ReadingSource = null;
        if (ReadingSource?.Length > MaxReadingSourceLength)
            ReadingSource = ReadingSource.Substring(0, MaxReadingSourceLength);

        Description = description?.Trim();
        if (Description?.Length > MaxDescriptionLength)
            Description = Description.Substring(0, MaxDescriptionLength);

        Notes = notes?.Trim();
        if (Notes?.Length > MaxNotesLength)
            Notes = Notes.Substring(0, MaxNotesLength);

        QueueDomainEvent(new ConsumptionDataCreated(Id, MeterId, ReadingDate, KWhUsed, BillingPeriod, Description, Notes));
    }

    /// <summary>
    /// Factory for creating a validated consumption record and computing kWh used.
    /// </summary>
    public static ConsumptionData Create(DefaultIdType meterId, DateTime readingDate,
        decimal currentReading, decimal previousReading, string billingPeriod,
        string readingType = "Actual", decimal? multiplier = null, string? readingSource = null,
        string? description = null, string? notes = null)
    {
        // Domain-level validation occurs in the private constructor
        return new ConsumptionData(meterId, readingDate, currentReading,
            previousReading, billingPeriod, readingType, multiplier, readingSource, description, notes);
    }

    /// <summary>
    /// Update readings and related metadata; recalculates <see cref="KWhUsed"/> and validity.
    /// </summary>
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

        if (!string.IsNullOrWhiteSpace(readingType) && ReadingType != readingType.Trim())
        {
            var rt = readingType.Trim();
            ReadingType = rt.Length > MaxReadingTypeLength ? rt.Substring(0, MaxReadingTypeLength) : rt;
            isUpdated = true;
        }

        if (multiplier.HasValue && Multiplier != multiplier.Value)
        {
            if (multiplier.Value <= 0)
                throw new ArgumentException("Multiplier must be greater than zero.");
            Multiplier = multiplier.Value;
            KWhUsed = CalculateUsage(CurrentReading, PreviousReading, Multiplier);
            isUpdated = true;
        }

        if (readingSource != ReadingSource)
        {
            var rs = readingSource?.Trim();
            if (!string.IsNullOrEmpty(rs) && rs.Length > MaxReadingSourceLength)
                rs = rs.Substring(0, MaxReadingSourceLength);
            ReadingSource = string.IsNullOrWhiteSpace(rs) ? null : rs;
            isUpdated = true;
        }

        if (description != Description)
        {
            var desc = description?.Trim();
            if (desc?.Length > MaxDescriptionLength)
                desc = desc.Substring(0, MaxDescriptionLength);
            Description = desc;
            isUpdated = true;
        }

        if (notes != Notes)
        {
            var nts = notes?.Trim();
            if (nts?.Length > MaxNotesLength)
                nts = nts.Substring(0, MaxNotesLength);
            Notes = nts;
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

    /// <summary>
    /// Mark this reading as estimated and append reason to notes.
    /// </summary>
    public ConsumptionData MarkAsEstimated(string reason)
    {
        ReadingType = "Estimated";
        Notes = string.IsNullOrWhiteSpace(Notes) ? $"Estimated: {reason}" : $"{Notes}; Estimated: {reason}";
        QueueDomainEvent(new ConsumptionDataMarkedAsEstimated(Id, MeterId, reason));
        return this;
    }
}