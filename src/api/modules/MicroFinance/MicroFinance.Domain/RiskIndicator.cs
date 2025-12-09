using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a risk indicator or Key Risk Indicator (KRI) for monitoring risk metrics.
/// Defines thresholds and measurement parameters for organizational risk management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define portfolio risk metrics (PAR30, PAR90, etc.).
/// - Set threshold triggers for risk alerts.
/// - Configure operational risk indicators.
/// - Track liquidity and solvency metrics.
/// - Support regulatory risk reporting.
/// 
/// Default values and constraints:
/// - Code: required unique identifier, max 32 characters (example: "KRI-PAR30")
/// - Name: required, max 128 characters (example: "Portfolio at Risk 30 Days")
/// - Formula: calculation logic, max 512 characters
/// - Unit: measurement unit (%, count, amount), max 32 characters
/// - Thresholds: Green (normal), Yellow (warning), Red (critical)
/// 
/// Business rules:
/// - KRI code must be unique.
/// - Thresholds define alert trigger levels.
/// - Direction indicates if higher/lower is better.
/// - Measurement frequency defined (daily, weekly, monthly).
/// - Breach of red threshold triggers immediate alert.
/// </remarks>
/// <seealso cref="RiskAlert"/>
/// <seealso cref="RiskCategory"/>
public sealed class RiskIndicator : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int Code = 32;
        public const int Name = 128;
        public const int Description = 4096;
        public const int Formula = 512;
        public const int Unit = 32;
        public const int DataSource = 128;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Indicator direction (whether higher or lower is better/worse).
    /// </summary>
    public const string DirectionHigherIsBetter = "HigherIsBetter";
    public const string DirectionLowerIsBetter = "LowerIsBetter";
    public const string DirectionNeutral = "Neutral";

    /// <summary>
    /// Measurement frequency.
    /// </summary>
    public const string FrequencyDaily = "Daily";
    public const string FrequencyWeekly = "Weekly";
    public const string FrequencyMonthly = "Monthly";
    public const string FrequencyQuarterly = "Quarterly";
    public const string FrequencyAnnual = "Annual";
    public const string FrequencyRealTime = "RealTime";

    /// <summary>
    /// Current status.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusDeprecated = "Deprecated";

    /// <summary>
    /// Current health status based on thresholds.
    /// </summary>
    public const string HealthGreen = "Green";
    public const string HealthYellow = "Yellow";
    public const string HealthOrange = "Orange";
    public const string HealthRed = "Red";

    /// <summary>
    /// Reference to the risk category.
    /// </summary>
    public Guid RiskCategoryId { get; private set; }

    /// <summary>
    /// Unique code for the indicator.
    /// </summary>
    public string Code { get; private set; } = string.Empty;


    /// <summary>
    /// Calculation formula (if applicable).
    /// </summary>
    public string? Formula { get; private set; }

    /// <summary>
    /// Unit of measurement.
    /// </summary>
    public string? Unit { get; private set; }

    /// <summary>
    /// Whether higher or lower values are better.
    /// </summary>
    public string Direction { get; private set; } = DirectionNeutral;

    /// <summary>
    /// Measurement frequency.
    /// </summary>
    public string Frequency { get; private set; } = FrequencyMonthly;

    /// <summary>
    /// Source of data for this indicator.
    /// </summary>
    public string? DataSource { get; private set; }

    /// <summary>
    /// Target value for the indicator.
    /// </summary>
    public decimal? TargetValue { get; private set; }

    /// <summary>
    /// Threshold for green status (acceptable).
    /// </summary>
    public decimal? GreenThreshold { get; private set; }

    /// <summary>
    /// Threshold for yellow status (warning).
    /// </summary>
    public decimal? YellowThreshold { get; private set; }

    /// <summary>
    /// Threshold for orange status (elevated).
    /// </summary>
    public decimal? OrangeThreshold { get; private set; }

    /// <summary>
    /// Threshold for red status (critical).
    /// </summary>
    public decimal? RedThreshold { get; private set; }

    /// <summary>
    /// Current measured value.
    /// </summary>
    public decimal? CurrentValue { get; private set; }

    /// <summary>
    /// Previous measured value (for trend analysis).
    /// </summary>
    public decimal? PreviousValue { get; private set; }

    /// <summary>
    /// Current health status based on thresholds.
    /// </summary>
    public string CurrentHealth { get; private set; } = HealthGreen;

    /// <summary>
    /// Timestamp of last measurement.
    /// </summary>
    public DateTime? LastMeasuredAt { get; private set; }

    /// <summary>
    /// Weight factor for composite scoring.
    /// </summary>
    public decimal WeightFactor { get; private set; } = 1.0m;

    /// <summary>
    /// Current status.
    /// </summary>
    public string Status { get; private set; } = StatusActive;
    
    // Navigation properties
    public RiskCategory RiskCategory { get; private set; } = null!;
    public ICollection<RiskAlert> Alerts { get; private set; } = new List<RiskAlert>();

    private RiskIndicator() { }

    /// <summary>
    /// Creates a new risk indicator.
    /// </summary>
    public static RiskIndicator Create(
        Guid riskCategoryId,
        string code,
        string name,
        string direction,
        string frequency,
        string? description = null,
        string? formula = null,
        string? unit = null,
        string? dataSource = null,
        decimal? targetValue = null,
        decimal? greenThreshold = null,
        decimal? yellowThreshold = null,
        decimal? orangeThreshold = null,
        decimal? redThreshold = null,
        decimal weightFactor = 1.0m)
    {
        var indicator = new RiskIndicator
        {
            RiskCategoryId = riskCategoryId,
            Code = code,
            Name = name,
            Direction = direction,
            Frequency = frequency,
            Description = description,
            Formula = formula,
            Unit = unit,
            DataSource = dataSource,
            TargetValue = targetValue,
            GreenThreshold = greenThreshold,
            YellowThreshold = yellowThreshold,
            OrangeThreshold = orangeThreshold,
            RedThreshold = redThreshold,
            WeightFactor = weightFactor,
            CurrentHealth = HealthGreen,
            Status = StatusActive
        };

        indicator.QueueDomainEvent(new RiskIndicatorCreated(indicator));
        return indicator;
    }

    /// <summary>
    /// Updates the risk indicator configuration.
    /// </summary>
    public RiskIndicator Update(
        string? name,
        string? description,
        string? formula,
        string? unit,
        string? dataSource,
        decimal? targetValue,
        decimal? greenThreshold,
        decimal? yellowThreshold,
        decimal? orangeThreshold,
        decimal? redThreshold,
        decimal? weightFactor,
        string? notes)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (formula is not null) Formula = formula;
        if (unit is not null) Unit = unit;
        if (dataSource is not null) DataSource = dataSource;
        if (targetValue.HasValue) TargetValue = targetValue.Value;
        if (greenThreshold.HasValue) GreenThreshold = greenThreshold.Value;
        if (yellowThreshold.HasValue) YellowThreshold = yellowThreshold.Value;
        if (orangeThreshold.HasValue) OrangeThreshold = orangeThreshold.Value;
        if (redThreshold.HasValue) RedThreshold = redThreshold.Value;
        if (weightFactor.HasValue) WeightFactor = weightFactor.Value;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new RiskIndicatorUpdated(this));
        return this;
    }

    /// <summary>
    /// Records a new measurement for the indicator.
    /// </summary>
    public void RecordMeasurement(decimal value)
    {
        PreviousValue = CurrentValue;
        CurrentValue = value;
        LastMeasuredAt = DateTime.UtcNow;

        var previousHealth = CurrentHealth;
        UpdateHealthStatus();

        QueueDomainEvent(new RiskIndicatorMeasured(Id, value, CurrentHealth));

        if (previousHealth != CurrentHealth)
        {
            QueueDomainEvent(new RiskIndicatorHealthChanged(Id, previousHealth, CurrentHealth));
        }
    }

    private void UpdateHealthStatus()
    {
        if (!CurrentValue.HasValue)
        {
            CurrentHealth = HealthGreen;
            return;
        }

        var value = CurrentValue.Value;

        // For "Lower is Better" direction
        if (Direction == DirectionLowerIsBetter)
        {
            if (RedThreshold.HasValue && value >= RedThreshold.Value)
                CurrentHealth = HealthRed;
            else if (OrangeThreshold.HasValue && value >= OrangeThreshold.Value)
                CurrentHealth = HealthOrange;
            else if (YellowThreshold.HasValue && value >= YellowThreshold.Value)
                CurrentHealth = HealthYellow;
            else
                CurrentHealth = HealthGreen;
        }
        // For "Higher is Better" direction
        else if (Direction == DirectionHigherIsBetter)
        {
            if (RedThreshold.HasValue && value <= RedThreshold.Value)
                CurrentHealth = HealthRed;
            else if (OrangeThreshold.HasValue && value <= OrangeThreshold.Value)
                CurrentHealth = HealthOrange;
            else if (YellowThreshold.HasValue && value <= YellowThreshold.Value)
                CurrentHealth = HealthYellow;
            else
                CurrentHealth = HealthGreen;
        }
        else
        {
            // Neutral - just check against thresholds
            CurrentHealth = HealthGreen;
        }
    }

    /// <summary>
    /// Activates the indicator.
    /// </summary>
    public void Activate()
    {
        if (Status == StatusActive) return;
        Status = StatusActive;
    }

    /// <summary>
    /// Deactivates the indicator.
    /// </summary>
    public void Deactivate()
    {
        Status = StatusInactive;
    }
}
