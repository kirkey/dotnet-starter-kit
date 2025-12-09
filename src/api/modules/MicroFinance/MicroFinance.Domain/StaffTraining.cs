using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents training records for staff members.
/// Tracks training attended, certifications earned, and ongoing professional development.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record staff training attendance and completion.
/// - Track mandatory compliance training requirements.
/// - Manage certifications and renewal dates.
/// - Identify training gaps for staff development.
/// - Support HR reporting and compliance audits.
/// 
/// Default values and constraints:
/// - TrainingCode: required unique identifier, max 32 characters (example: "TRN-AML-001")
/// - TrainingName: required, max 256 characters (example: "AML/CFT Compliance Training")
/// - TrainingType: Onboarding, Compliance, Product, SoftSkills, Technical
/// - Status: Enrolled by default (Enrolled, InProgress, Completed, Failed, Cancelled)
/// - Provider: training provider name, max 128 characters
/// - CertificationNumber: certification ID if applicable, max 64 characters
/// 
/// Business rules:
/// - Compliance training has mandatory completion deadlines.
/// - Certifications have expiry dates requiring renewal.
/// - Training completion tracked for performance reviews.
/// - Minimum training hours required per staff role.
/// - Failed training requires re-enrollment.
/// </remarks>
/// <seealso cref="Staff"/>
public sealed class StaffTraining : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Maximum length constants for string properties.
    /// </summary>
    public static class MaxLengths
    {
        public const int TrainingName = 256;
        public const int TrainingCode = 32;
        public const int TrainingType = 64;
        public const int Provider = 128;
        public const int Location = 256;
        public const int CertificationNumber = 64;
        public const int Description = 4096;
        public const int Notes = 4096;
    }

    /// <summary>
    /// Training type classification.
    /// </summary>
    public const string TypeOnboarding = "Onboarding";
    public const string TypeCompliance = "Compliance";
    public const string TypeProduct = "Product";
    public const string TypeSoftSkills = "SoftSkills";
    public const string TypeTechnical = "Technical";
    public const string TypeLeadership = "Leadership";
    public const string TypeCertification = "Certification";
    public const string TypeRefresher = "Refresher";

    /// <summary>
    /// Delivery method.
    /// </summary>
    public const string MethodClassroom = "Classroom";
    public const string MethodOnline = "Online";
    public const string MethodOnTheJob = "OnTheJob";
    public const string MethodWorkshop = "Workshop";
    public const string MethodSeminar = "Seminar";
    public const string MethodSelfPaced = "SelfPaced";

    /// <summary>
    /// Training completion status.
    /// </summary>
    public const string StatusScheduled = "Scheduled";
    public const string StatusInProgress = "InProgress";
    public const string StatusCompleted = "Completed";
    public const string StatusCancelled = "Cancelled";
    public const string StatusFailed = "Failed";
    public const string StatusExpired = "Expired";

    /// <summary>
    /// Reference to the staff member.
    /// </summary>
    public Guid StaffId { get; private set; }

    /// <summary>
    /// Unique training code.
    /// </summary>
    public string? TrainingCode { get; private set; }

    /// <summary>
    /// Name of the training program.
    /// </summary>
    public string TrainingName { get; private set; } = string.Empty;

    /// <summary>
    /// Description of the training content.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Type of training.
    /// </summary>
    public string TrainingType { get; private set; } = TypeOnboarding;

    /// <summary>
    /// Delivery method.
    /// </summary>
    public string DeliveryMethod { get; private set; } = MethodClassroom;

    /// <summary>
    /// Training provider or institution.
    /// </summary>
    public string? Provider { get; private set; }

    /// <summary>
    /// Location of the training.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Start date of the training.
    /// </summary>
    public DateOnly StartDate { get; private set; }

    /// <summary>
    /// End date of the training.
    /// </summary>
    public DateOnly? EndDate { get; private set; }

    /// <summary>
    /// Duration in hours.
    /// </summary>
    public int? DurationHours { get; private set; }

    /// <summary>
    /// Score or grade achieved (percentage).
    /// </summary>
    public decimal? Score { get; private set; }

    /// <summary>
    /// Passing score required.
    /// </summary>
    public decimal? PassingScore { get; private set; }

    /// <summary>
    /// Whether a certificate was issued.
    /// </summary>
    public bool CertificateIssued { get; private set; }

    /// <summary>
    /// Certificate or certification number.
    /// </summary>
    public string? CertificationNumber { get; private set; }

    /// <summary>
    /// Date the certification was issued.
    /// </summary>
    public DateOnly? CertificationDate { get; private set; }

    /// <summary>
    /// Expiry date of the certification.
    /// </summary>
    public DateOnly? CertificationExpiryDate { get; private set; }

    /// <summary>
    /// Cost of the training.
    /// </summary>
    public decimal? TrainingCost { get; private set; }

    /// <summary>
    /// Whether training is mandatory.
    /// </summary>
    public bool IsMandatory { get; private set; }

    /// <summary>
    /// Current status of the training.
    /// </summary>
    public string Status { get; private set; } = StatusScheduled;

    /// <summary>
    /// Completion date.
    /// </summary>
    public DateOnly? CompletionDate { get; private set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; private set; }

    // Navigation properties
    public Staff Staff { get; private set; } = null!;

    private StaffTraining() { }

    /// <summary>
    /// Schedules a new training for a staff member.
    /// </summary>
    public static StaffTraining Schedule(
        Guid staffId,
        string trainingName,
        string trainingType,
        string deliveryMethod,
        DateOnly startDate,
        DateOnly? endDate = null,
        int? durationHours = null,
        string? trainingCode = null,
        string? provider = null,
        string? location = null,
        string? description = null,
        bool isMandatory = false,
        decimal? trainingCost = null,
        decimal? passingScore = null)
    {
        var training = new StaffTraining
        {
            StaffId = staffId,
            TrainingName = trainingName,
            TrainingType = trainingType,
            DeliveryMethod = deliveryMethod,
            StartDate = startDate,
            EndDate = endDate,
            DurationHours = durationHours,
            TrainingCode = trainingCode,
            Provider = provider,
            Location = location,
            Description = description,
            IsMandatory = isMandatory,
            TrainingCost = trainingCost,
            PassingScore = passingScore,
            Status = StatusScheduled
        };

        training.QueueDomainEvent(new StaffTrainingScheduled(training));
        return training;
    }

    /// <summary>
    /// Updates training details.
    /// </summary>
    public StaffTraining Update(
        string? trainingName,
        string? description,
        DateOnly? startDate,
        DateOnly? endDate,
        int? durationHours,
        string? provider,
        string? location,
        decimal? trainingCost,
        string? notes)
    {
        if (trainingName is not null) TrainingName = trainingName;
        if (description is not null) Description = description;
        if (startDate.HasValue) StartDate = startDate.Value;
        if (endDate.HasValue) EndDate = endDate.Value;
        if (durationHours.HasValue) DurationHours = durationHours.Value;
        if (provider is not null) Provider = provider;
        if (location is not null) Location = location;
        if (trainingCost.HasValue) TrainingCost = trainingCost.Value;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new StaffTrainingUpdated(this));
        return this;
    }

    /// <summary>
    /// Marks training as in progress.
    /// </summary>
    public void Start()
    {
        if (Status != StatusScheduled)
            throw new InvalidOperationException("Only scheduled training can be started.");

        Status = StatusInProgress;
        QueueDomainEvent(new StaffTrainingStarted(Id));
    }

    /// <summary>
    /// Completes the training with results.
    /// </summary>
    public void Complete(decimal? score = null, DateOnly? completionDate = null)
    {
        if (Status != StatusInProgress && Status != StatusScheduled)
            throw new InvalidOperationException("Training must be in progress to complete.");

        Score = score;
        CompletionDate = completionDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        // Check if passed
        if (PassingScore.HasValue && score.HasValue && score.Value < PassingScore.Value)
        {
            Status = StatusFailed;
            QueueDomainEvent(new StaffTrainingFailed(Id, score.Value, PassingScore.Value));
        }
        else
        {
            Status = StatusCompleted;
            QueueDomainEvent(new StaffTrainingCompleted(Id, score, CompletionDate.Value));
        }
    }

    /// <summary>
    /// Issues a certificate for the completed training.
    /// </summary>
    public void IssueCertificate(
        string certificationNumber,
        DateOnly? certificationDate = null,
        DateOnly? expiryDate = null)
    {
        if (Status != StatusCompleted)
            throw new InvalidOperationException("Certificate can only be issued for completed training.");

        CertificateIssued = true;
        CertificationNumber = certificationNumber;
        CertificationDate = certificationDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        CertificationExpiryDate = expiryDate;

        QueueDomainEvent(new StaffTrainingCertificateIssued(Id, certificationNumber, expiryDate));
    }

    /// <summary>
    /// Marks the certification as expired.
    /// </summary>
    public void MarkExpired()
    {
        if (!CertificateIssued)
            throw new InvalidOperationException("No certificate to expire.");

        Status = StatusExpired;
        QueueDomainEvent(new StaffTrainingCertificationExpired(Id, CertificationNumber!));
    }

    /// <summary>
    /// Cancels the scheduled training.
    /// </summary>
    public void Cancel(string? reason = null)
    {
        if (Status == StatusCompleted)
            throw new InvalidOperationException("Completed training cannot be cancelled.");

        Status = StatusCancelled;
        if (reason is not null) Notes = reason;
        QueueDomainEvent(new StaffTrainingCancelled(Id, reason));
    }
}
