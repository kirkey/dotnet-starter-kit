using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a credit bureau report containing a member's credit history.
/// Stores detailed credit information retrieved from credit bureaus.
/// </summary>
public sealed class CreditBureauReport : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int ReportNumber = 64;
        public const int BureauName = 128;
        public const int ScoreModel = 64;
        public const int RiskGrade = 32;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Score grade classifications.
    /// </summary>
    public const string GradeExcellent = "Excellent";
    public const string GradeGood = "Good";
    public const string GradeFair = "Fair";
    public const string GradePoor = "Poor";
    public const string GradeVeryPoor = "VeryPoor";
    public const string GradeNoScore = "NoScore";

    /// <summary>
    /// Report status.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusExpired = "Expired";
    public const string StatusDisputed = "Disputed";

    /// <summary>
    /// Reference to the member.
    /// </summary>
    public Guid MemberId { get; private set; }

    /// <summary>
    /// Reference to the inquiry that generated this report.
    /// </summary>
    public Guid? InquiryId { get; private set; }

    /// <summary>
    /// Unique report number from the bureau.
    /// </summary>
    public string ReportNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Name of the credit bureau.
    /// </summary>
    public string BureauName { get; private set; } = string.Empty;

    /// <summary>
    /// Date when the report was generated.
    /// </summary>
    public DateTime ReportDate { get; private set; }

    /// <summary>
    /// Expiry date of the report validity.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }

    /// <summary>
    /// Credit score from the report.
    /// </summary>
    public int? CreditScore { get; private set; }

    /// <summary>
    /// Minimum possible score for the model.
    /// </summary>
    public int? ScoreMin { get; private set; }

    /// <summary>
    /// Maximum possible score for the model.
    /// </summary>
    public int? ScoreMax { get; private set; }

    /// <summary>
    /// Scoring model used.
    /// </summary>
    public string? ScoreModel { get; private set; }

    /// <summary>
    /// Risk grade based on the score.
    /// </summary>
    public string? RiskGrade { get; private set; }

    /// <summary>
    /// Number of active credit accounts.
    /// </summary>
    public int? ActiveAccounts { get; private set; }

    /// <summary>
    /// Number of closed credit accounts.
    /// </summary>
    public int? ClosedAccounts { get; private set; }

    /// <summary>
    /// Number of delinquent accounts.
    /// </summary>
    public int? DelinquentAccounts { get; private set; }

    /// <summary>
    /// Total outstanding balance across all accounts.
    /// </summary>
    public decimal? TotalOutstandingBalance { get; private set; }

    /// <summary>
    /// Total credit limit across all accounts.
    /// </summary>
    public decimal? TotalCreditLimit { get; private set; }

    /// <summary>
    /// Credit utilization percentage.
    /// </summary>
    public decimal? CreditUtilization { get; private set; }

    /// <summary>
    /// Number of hard inquiries in the past 12 months.
    /// </summary>
    public int? RecentInquiries { get; private set; }

    /// <summary>
    /// Length of credit history in months.
    /// </summary>
    public int? CreditHistoryMonths { get; private set; }

    /// <summary>
    /// Number of late payments in the past 12 months.
    /// </summary>
    public int? LatePayments12Months { get; private set; }

    /// <summary>
    /// Number of late payments in the past 24 months.
    /// </summary>
    public int? LatePayments24Months { get; private set; }

    /// <summary>
    /// Number of defaults or charge-offs.
    /// </summary>
    public int? Defaults { get; private set; }

    /// <summary>
    /// Number of bankruptcies on record.
    /// </summary>
    public int? Bankruptcies { get; private set; }

    /// <summary>
    /// Number of collections on record.
    /// </summary>
    public int? Collections { get; private set; }

    /// <summary>
    /// Public records (liens, judgments, etc.).
    /// </summary>
    public int? PublicRecords { get; private set; }

    /// <summary>
    /// Debt-to-income ratio if provided.
    /// </summary>
    public decimal? DebtToIncomeRatio { get; private set; }

    /// <summary>
    /// Raw report data in JSON format.
    /// </summary>
    public string? RawReportData { get; private set; }

    /// <summary>
    /// Current status of the report.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Member Member { get; private set; } = null!;
    public CreditBureauInquiry? Inquiry { get; private set; }

    private CreditBureauReport() { }

    /// <summary>
    /// Creates a new credit bureau report.
    /// </summary>
    public static CreditBureauReport Create(
        Guid memberId,
        string reportNumber,
        string bureauName,
        DateTime reportDate,
        Guid? inquiryId = null,
        int? creditScore = null,
        int? scoreMin = null,
        int? scoreMax = null,
        string? scoreModel = null,
        DateTime? expiryDate = null)
    {
        var report = new CreditBureauReport
        {
            MemberId = memberId,
            ReportNumber = reportNumber,
            BureauName = bureauName,
            ReportDate = reportDate,
            InquiryId = inquiryId,
            CreditScore = creditScore,
            ScoreMin = scoreMin,
            ScoreMax = scoreMax,
            ScoreModel = scoreModel,
            ExpiryDate = expiryDate ?? DateTime.UtcNow.AddDays(90),
            Status = StatusActive
        };

        report.UpdateRiskGrade();
        report.QueueDomainEvent(new CreditBureauReportCreated(report));
        return report;
    }

    /// <summary>
    /// Populates the report with credit account summary data.
    /// </summary>
    public void PopulateAccountSummary(
        int? activeAccounts,
        int? closedAccounts,
        int? delinquentAccounts,
        decimal? totalOutstandingBalance,
        decimal? totalCreditLimit,
        int? recentInquiries,
        int? creditHistoryMonths)
    {
        ActiveAccounts = activeAccounts;
        ClosedAccounts = closedAccounts;
        DelinquentAccounts = delinquentAccounts;
        TotalOutstandingBalance = totalOutstandingBalance;
        TotalCreditLimit = totalCreditLimit;
        RecentInquiries = recentInquiries;
        CreditHistoryMonths = creditHistoryMonths;

        if (totalCreditLimit.HasValue && totalCreditLimit.Value > 0 && totalOutstandingBalance.HasValue)
        {
            CreditUtilization = Math.Round((totalOutstandingBalance.Value / totalCreditLimit.Value) * 100, 2);
        }

        QueueDomainEvent(new CreditBureauReportUpdated(this));
    }

    /// <summary>
    /// Populates the report with payment history data.
    /// </summary>
    public void PopulatePaymentHistory(
        int? latePayments12Months,
        int? latePayments24Months,
        int? defaults,
        int? bankruptcies,
        int? collections,
        int? publicRecords)
    {
        LatePayments12Months = latePayments12Months;
        LatePayments24Months = latePayments24Months;
        Defaults = defaults;
        Bankruptcies = bankruptcies;
        Collections = collections;
        PublicRecords = publicRecords;

        QueueDomainEvent(new CreditBureauReportUpdated(this));
    }

    /// <summary>
    /// Sets the raw report data.
    /// </summary>
    public void SetRawData(string rawReportData)
    {
        RawReportData = rawReportData;
    }

    private void UpdateRiskGrade()
    {
        if (!CreditScore.HasValue)
        {
            RiskGrade = GradeNoScore;
            return;
        }

        // Default scoring model: 300-850 range
        var score = CreditScore.Value;
        var min = ScoreMin ?? 300;
        var max = ScoreMax ?? 850;
        var range = max - min;

        // Calculate percentile within the range
        var percentile = ((decimal)(score - min) / range) * 100;

        RiskGrade = percentile switch
        {
            >= 80 => GradeExcellent,
            >= 60 => GradeGood,
            >= 40 => GradeFair,
            >= 20 => GradePoor,
            _ => GradeVeryPoor
        };
    }

    /// <summary>
    /// Marks the report as expired.
    /// </summary>
    public void MarkExpired()
    {
        Status = StatusExpired;
        QueueDomainEvent(new CreditBureauReportExpired(Id));
    }

    /// <summary>
    /// Marks the report as disputed.
    /// </summary>
    public void Dispute(string reason)
    {
        Status = StatusDisputed;
        Notes = reason;
        QueueDomainEvent(new CreditBureauReportDisputed(Id, reason));
    }

    /// <summary>
    /// Checks if the report is still valid.
    /// </summary>
    public bool IsValid()
    {
        if (Status != StatusActive) return false;
        if (ExpiryDate.HasValue && DateTime.UtcNow > ExpiryDate.Value) return false;
        return true;
    }
}
