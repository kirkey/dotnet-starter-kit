using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents performance targets assigned to a branch for a specific period.
/// Used to track and measure branch performance against organizational goals.
/// </summary>
/// <remarks>
/// Use cases:
/// - Set annual, quarterly, and monthly branch performance targets.
/// - Track actual vs. budgeted performance across key metrics.
/// - Cascade organizational targets down to branch level.
/// - Compare branch performance for ranking and resource allocation.
/// - Support strategic planning and branch expansion decisions.
/// 
/// Default values and constraints:
/// - TargetType: LoanDisbursement, LoanCollection, NewMembers, SavingsDeposits, PortfolioAtRisk, Revenue.
/// - Period: Monthly, Quarterly, Yearly.
/// - TargetValue: Target amount or percentage.
/// - ActualValue: Achieved amount (updated as performance occurs).
/// - StartDate: Period start date (required).
/// - EndDate: Period end date (required).
/// - AchievementPercentage: Calculated as (Actual/Target) × 100.
/// 
/// Business rules:
/// - Drives operational excellence across MFI network.
/// - Growth: New members, loans, and savings mobilization.
/// - Quality: Portfolio at risk, on-time repayment rates.
/// - Efficiency: Operating costs, revenue targets.
/// - Products: Insurance sales, share capital growth.
/// - Used for branch ranking and resource allocation.
/// </remarks>
/// <seealso cref="Branch"/>
/// <seealso cref="LoanOfficerTarget"/>
/// <example>
/// <para><strong>Example: Setting branch quarterly targets</strong></para>
/// <code>
/// POST /api/microfinance/branch-targets
/// {
///   "branchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "targetType": "LoanDisbursement",
///   "period": "Quarterly",
///   "startDate": "2024-01-01",
///   "endDate": "2024-03-31",
///   "targetValue": 150000000,
///   "metricUnit": "RWF",
///   "description": "Q1 2024 Kigali Main Branch disbursement target"
/// }
/// </code>
/// </example>
public sealed class BranchTarget : AuditableEntity, IAggregateRoot
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
    public const string TypeNewGroups = "NewGroups";
    public const string TypeSavingsDeposits = "SavingsDeposits";
    public const string TypeNewLoans = "NewLoans";
    public const string TypeRecoveryRate = "RecoveryRate";
    public const string TypePortfolioAtRisk = "PortfolioAtRisk";
    public const string TypeOperatingCost = "OperatingCost";
    public const string TypeRevenue = "Revenue";
    public const string TypeInsuranceSales = "InsuranceSales";

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
    /// Reference to the branch this target is for.
    /// </summary>
    public Guid BranchId { get; private set; }

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
    /// Additional notes or context.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Branch Branch { get; private set; } = null!;

    private BranchTarget() { }

    /// <summary>
    /// Creates a new branch target.
    /// </summary>
    public static BranchTarget Create(
        Guid branchId,
        string targetType,
        string period,
        DateOnly periodStart,
        DateOnly periodEnd,
        decimal targetValue,
        string? metricUnit = null,
        string? description = null,
        decimal? minimumThreshold = null,
        decimal? stretchTarget = null,
        decimal weight = 1.0m)
    {
        var target = new BranchTarget
        {
            BranchId = branchId,
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
            AchievedValue = 0,
            AchievementPercentage = 0,
            Status = StatusNotStarted
        };

        target.QueueDomainEvent(new BranchTargetCreated(target));
        return target;
    }

    /// <summary>
    /// Updates the target configuration.
    /// </summary>
    public BranchTarget Update(
        decimal? targetValue,
        string? description,
        decimal? minimumThreshold,
        decimal? stretchTarget,
        decimal? weight,
        string? notes)
    {
        if (targetValue.HasValue) TargetValue = targetValue.Value;
        if (description is not null) Description = description;
        if (minimumThreshold.HasValue) MinimumThreshold = minimumThreshold.Value;
        if (stretchTarget.HasValue) StretchTarget = stretchTarget.Value;
        if (weight.HasValue) Weight = weight.Value;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new BranchTargetUpdated(this));
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
        QueueDomainEvent(new BranchTargetProgressRecorded(Id, previousValue, achievedValue, AchievementPercentage));
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
}
