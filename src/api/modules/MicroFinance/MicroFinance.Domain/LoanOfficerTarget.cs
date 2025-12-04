using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents performance targets assigned to a loan officer for a specific period.
/// Used for individual performance tracking and incentive calculations.
/// </summary>
public sealed class LoanOfficerTarget : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int TargetType = 64;
        public const int Description = 512;
        public const int Period = 32;
        public const int MetricUnit = 32;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Target type categories.
    /// </summary>
    public const string TypeLoanDisbursement = "LoanDisbursement";
    public const string TypeLoanCollection = "LoanCollection";
    public const string TypeNewMembers = "NewMembers";
    public const string TypeNewLoans = "NewLoans";
    public const string TypeRecoveryRate = "RecoveryRate";
    public const string TypePortfolioAtRisk = "PortfolioAtRisk";
    public const string TypeClientRetention = "ClientRetention";
    public const string TypeGroupMeetings = "GroupMeetings";
    public const string TypeFieldVisits = "FieldVisits";
    public const string TypeOnTimeRepayment = "OnTimeRepayment";

    /// <summary>
    /// Period type for the target.
    /// </summary>
    public const string PeriodDaily = "Daily";
    public const string PeriodWeekly = "Weekly";
    public const string PeriodMonthly = "Monthly";
    public const string PeriodQuarterly = "Quarterly";
    public const string PeriodAnnual = "Annual";

    /// <summary>
    /// Status of target achievement.
    /// </summary>
    public const string StatusNotStarted = "NotStarted";
    public const string StatusInProgress = "InProgress";
    public const string StatusAchieved = "Achieved";
    public const string StatusMissed = "Missed";
    public const string StatusExceeded = "Exceeded";

    /// <summary>
    /// Reference to the staff member (loan officer).
    /// </summary>
    public Guid StaffId { get; private set; }

    /// <summary>
    /// Type of target metric.
    /// </summary>
    public string TargetType { get; private set; } = string.Empty;

    /// <summary>
    /// Description of the target.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Period type (Monthly, Quarterly, Annual, etc.).
    /// </summary>
    public string Period { get; private set; } = PeriodMonthly;

    /// <summary>
    /// Start date of the target period.
    /// </summary>
    public DateOnly PeriodStart { get; private set; }

    /// <summary>
    /// End date of the target period.
    /// </summary>
    public DateOnly PeriodEnd { get; private set; }

    /// <summary>
    /// Target value to achieve.
    /// </summary>
    public decimal TargetValue { get; private set; }

    /// <summary>
    /// Unit of measurement for the target.
    /// </summary>
    public string? MetricUnit { get; private set; }

    /// <summary>
    /// Current achieved value.
    /// </summary>
    public decimal AchievedValue { get; private set; }

    /// <summary>
    /// Achievement percentage (AchievedValue / TargetValue * 100).
    /// </summary>
    public decimal AchievementPercentage { get; private set; }

    /// <summary>
    /// Current status of the target.
    /// </summary>
    public string Status { get; private set; } = StatusNotStarted;

    /// <summary>
    /// Minimum threshold for acceptable performance.
    /// </summary>
    public decimal? MinimumThreshold { get; private set; }

    /// <summary>
    /// Stretch target for exceptional performance.
    /// </summary>
    public decimal? StretchTarget { get; private set; }

    /// <summary>
    /// Weight of this target in overall performance score.
    /// </summary>
    public decimal Weight { get; private set; } = 1.0m;

    /// <summary>
    /// Incentive amount for achieving the target.
    /// </summary>
    public decimal? IncentiveAmount { get; private set; }

    /// <summary>
    /// Bonus amount for exceeding stretch target.
    /// </summary>
    public decimal? StretchBonus { get; private set; }

    /// <summary>
    /// Additional notes or context.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Staff Staff { get; private set; } = null!;

    private LoanOfficerTarget() { }

    /// <summary>
    /// Creates a new loan officer target.
    /// </summary>
    public static LoanOfficerTarget Create(
        Guid staffId,
        string targetType,
        string period,
        DateOnly periodStart,
        DateOnly periodEnd,
        decimal targetValue,
        string? metricUnit = null,
        string? description = null,
        decimal? minimumThreshold = null,
        decimal? stretchTarget = null,
        decimal weight = 1.0m,
        decimal? incentiveAmount = null,
        decimal? stretchBonus = null)
    {
        var target = new LoanOfficerTarget
        {
            StaffId = staffId,
            TargetType = targetType,
            Period = period,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            TargetValue = targetValue,
            MetricUnit = metricUnit,
            Description = description,
            MinimumThreshold = minimumThreshold,
            StretchTarget = stretchTarget,
            Weight = weight,
            IncentiveAmount = incentiveAmount,
            StretchBonus = stretchBonus,
            AchievedValue = 0,
            AchievementPercentage = 0,
            Status = StatusNotStarted
        };

        target.QueueDomainEvent(new LoanOfficerTargetCreated(target));
        return target;
    }

    /// <summary>
    /// Updates the target configuration.
    /// </summary>
    public LoanOfficerTarget Update(
        decimal? targetValue,
        string? description,
        decimal? minimumThreshold,
        decimal? stretchTarget,
        decimal? weight,
        decimal? incentiveAmount,
        decimal? stretchBonus,
        string? notes)
    {
        if (targetValue.HasValue) TargetValue = targetValue.Value;
        if (description is not null) Description = description;
        if (minimumThreshold.HasValue) MinimumThreshold = minimumThreshold.Value;
        if (stretchTarget.HasValue) StretchTarget = stretchTarget.Value;
        if (weight.HasValue) Weight = weight.Value;
        if (incentiveAmount.HasValue) IncentiveAmount = incentiveAmount.Value;
        if (stretchBonus.HasValue) StretchBonus = stretchBonus.Value;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new LoanOfficerTargetUpdated(this));
        return this;
    }

    /// <summary>
    /// Records progress towards the target.
    /// </summary>
    public void RecordProgress(decimal achievedValue)
    {
        var previousValue = AchievedValue;
        AchievedValue = achievedValue;

        if (TargetValue > 0)
        {
            AchievementPercentage = Math.Round((achievedValue / TargetValue) * 100, 2);
        }

        UpdateStatus();
        QueueDomainEvent(new LoanOfficerTargetProgressRecorded(Id, previousValue, achievedValue, AchievementPercentage));
    }

    /// <summary>
    /// Increments the achieved value by a delta.
    /// </summary>
    public void IncrementProgress(decimal delta)
    {
        RecordProgress(AchievedValue + delta);
    }

    private void UpdateStatus()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (today < PeriodStart)
        {
            Status = StatusNotStarted;
        }
        else if (today > PeriodEnd)
        {
            // Period ended - determine final status
            if (StretchTarget.HasValue && AchievedValue >= StretchTarget.Value)
            {
                Status = StatusExceeded;
            }
            else if (AchievedValue >= TargetValue)
            {
                Status = StatusAchieved;
            }
            else
            {
                Status = StatusMissed;
            }
        }
        else
        {
            // Period in progress
            if (StretchTarget.HasValue && AchievedValue >= StretchTarget.Value)
            {
                Status = StatusExceeded;
            }
            else if (AchievedValue >= TargetValue)
            {
                Status = StatusAchieved;
            }
            else
            {
                Status = StatusInProgress;
            }
        }
    }

    /// <summary>
    /// Calculates the earned incentive based on achievement.
    /// </summary>
    public decimal CalculateEarnedIncentive()
    {
        if (!IncentiveAmount.HasValue) return 0;

        if (Status == StatusExceeded && StretchBonus.HasValue)
        {
            return IncentiveAmount.Value + StretchBonus.Value;
        }
        else if (Status == StatusAchieved || Status == StatusExceeded)
        {
            return IncentiveAmount.Value;
        }
        else if (Status == StatusInProgress && MinimumThreshold.HasValue && AchievedValue >= MinimumThreshold.Value)
        {
            // Prorated incentive for meeting minimum threshold
            var prorationFactor = AchievementPercentage / 100;
            return IncentiveAmount.Value * prorationFactor;
        }

        return 0;
    }
}
