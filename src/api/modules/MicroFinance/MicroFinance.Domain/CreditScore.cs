using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a credit score record for a member.
/// Tracks internal and external credit scoring over time.
/// </summary>
public sealed class CreditScore : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int ScoreType = 64;
        public const int ScoreModel = 64;
        public const int Grade = 32;
        public const int Source = 128;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Score type classification.
    /// </summary>
    public const string TypeInternal = "Internal";
    public const string TypeExternal = "External";
    public const string TypeBehavioral = "Behavioral";
    public const string TypeApplication = "Application";
    public const string TypeCollection = "Collection";

    /// <summary>
    /// Score grade classifications.
    /// </summary>
    public const string GradeA = "A";
    public const string GradeB = "B";
    public const string GradeC = "C";
    public const string GradeD = "D";
    public const string GradeE = "E";
    public const string GradeF = "F";

    /// <summary>
    /// Status of the score.
    /// </summary>
    public const string StatusActive = "Active";
    public const string StatusSuperseded = "Superseded";
    public const string StatusExpired = "Expired";

    /// <summary>
    /// Reference to the member.
    /// </summary>
    public Guid MemberId { get; private set; }

    /// <summary>
    /// Reference to the associated loan (if loan-specific scoring).
    /// </summary>
    public Guid? LoanId { get; private set; }

    /// <summary>
    /// Type of credit score.
    /// </summary>
    public string ScoreType { get; private set; } = TypeInternal;

    /// <summary>
    /// Scoring model or algorithm used.
    /// </summary>
    public string? ScoreModel { get; private set; }

    /// <summary>
    /// The numeric credit score.
    /// </summary>
    public decimal Score { get; private set; }

    /// <summary>
    /// Minimum possible score for this model.
    /// </summary>
    public decimal ScoreMin { get; private set; }

    /// <summary>
    /// Maximum possible score for this model.
    /// </summary>
    public decimal ScoreMax { get; private set; }

    /// <summary>
    /// Calculated score percentile.
    /// </summary>
    public decimal ScorePercentile { get; private set; }

    /// <summary>
    /// Grade classification based on the score.
    /// </summary>
    public string Grade { get; private set; } = GradeC;

    /// <summary>
    /// Probability of default (if calculated).
    /// </summary>
    public decimal? ProbabilityOfDefault { get; private set; }

    /// <summary>
    /// Expected loss given default.
    /// </summary>
    public decimal? LossGivenDefault { get; private set; }

    /// <summary>
    /// Expected exposure at default.
    /// </summary>
    public decimal? ExposureAtDefault { get; private set; }

    /// <summary>
    /// Expected loss (PD x LGD x EAD).
    /// </summary>
    public decimal? ExpectedLoss { get; private set; }

    /// <summary>
    /// Date when the score was calculated.
    /// </summary>
    public DateTime ScoredAt { get; private set; }

    /// <summary>
    /// Date when this score is valid until.
    /// </summary>
    public DateTime? ValidUntil { get; private set; }

    /// <summary>
    /// Source of the score (Bureau name or "Internal").
    /// </summary>
    public string? Source { get; private set; }

    /// <summary>
    /// Reference to the credit bureau report (if external).
    /// </summary>
    public Guid? CreditBureauReportId { get; private set; }

    /// <summary>
    /// Factors that contributed to the score (JSON format).
    /// </summary>
    public string? ScoreFactors { get; private set; }

    /// <summary>
    /// Score change from previous score.
    /// </summary>
    public decimal? ScoreChange { get; private set; }

    /// <summary>
    /// Previous score ID for tracking history.
    /// </summary>
    public Guid? PreviousScoreId { get; private set; }

    /// <summary>
    /// Current status of the score.
    /// </summary>
    public string Status { get; private set; } = StatusActive;

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Member Member { get; private set; } = null!;
    public Loan? Loan { get; private set; }
    public CreditBureauReport? CreditBureauReport { get; private set; }
    public CreditScore? PreviousScore { get; private set; }

    private CreditScore() { }

    /// <summary>
    /// Creates a new credit score record.
    /// </summary>
    public static CreditScore Create(
        Guid memberId,
        string scoreType,
        decimal score,
        decimal scoreMin,
        decimal scoreMax,
        string? scoreModel = null,
        Guid? loanId = null,
        string? source = null,
        Guid? creditBureauReportId = null,
        decimal? probabilityOfDefault = null,
        string? scoreFactors = null,
        DateTime? validUntil = null,
        Guid? previousScoreId = null,
        decimal? previousScoreValue = null)
    {
        var creditScore = new CreditScore
        {
            MemberId = memberId,
            ScoreType = scoreType,
            Score = score,
            ScoreMin = scoreMin,
            ScoreMax = scoreMax,
            ScoreModel = scoreModel,
            LoanId = loanId,
            Source = source,
            CreditBureauReportId = creditBureauReportId,
            ProbabilityOfDefault = probabilityOfDefault,
            ScoreFactors = scoreFactors,
            ScoredAt = DateTime.UtcNow,
            ValidUntil = validUntil,
            PreviousScoreId = previousScoreId,
            Status = StatusActive
        };

        // Calculate percentile
        var range = scoreMax - scoreMin;
        if (range > 0)
        {
            creditScore.ScorePercentile = Math.Round(((score - scoreMin) / range) * 100, 2);
        }

        // Determine grade
        creditScore.Grade = creditScore.ScorePercentile switch
        {
            >= 90 => GradeA,
            >= 75 => GradeB,
            >= 60 => GradeC,
            >= 40 => GradeD,
            >= 20 => GradeE,
            _ => GradeF
        };

        // Calculate score change if previous score provided
        if (previousScoreValue.HasValue)
        {
            creditScore.ScoreChange = score - previousScoreValue.Value;
        }

        creditScore.QueueDomainEvent(new CreditScoreCreated(creditScore));
        return creditScore;
    }

    /// <summary>
    /// Sets the loss parameters for risk calculation.
    /// </summary>
    public void SetLossParameters(
        decimal? probabilityOfDefault,
        decimal? lossGivenDefault,
        decimal? exposureAtDefault)
    {
        ProbabilityOfDefault = probabilityOfDefault;
        LossGivenDefault = lossGivenDefault;
        ExposureAtDefault = exposureAtDefault;

        if (probabilityOfDefault.HasValue && lossGivenDefault.HasValue && exposureAtDefault.HasValue)
        {
            ExpectedLoss = probabilityOfDefault.Value * lossGivenDefault.Value * exposureAtDefault.Value;
        }

        QueueDomainEvent(new CreditScoreUpdated(this));
    }

    /// <summary>
    /// Marks this score as superseded by a newer score.
    /// </summary>
    public void Supersede()
    {
        if (Status != StatusActive) return;
        Status = StatusSuperseded;
        QueueDomainEvent(new CreditScoreSuperseded(Id));
    }

    /// <summary>
    /// Marks the score as expired.
    /// </summary>
    public void MarkExpired()
    {
        if (Status != StatusActive) return;
        Status = StatusExpired;
        QueueDomainEvent(new CreditScoreExpired(Id));
    }

    /// <summary>
    /// Checks if the score is still valid.
    /// </summary>
    public bool IsValid()
    {
        if (Status != StatusActive) return false;
        if (ValidUntil.HasValue && DateTime.UtcNow > ValidUntil.Value) return false;
        return true;
    }
}
