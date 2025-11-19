namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

using Events;

/// <summary>
/// Represents an employee performance review.
/// Tracks performance evaluations, ratings, goals, and feedback.
/// </summary>
public class PerformanceReview : AuditableEntity, IAggregateRoot
{
    private PerformanceReview() { }

    private PerformanceReview(
        DefaultIdType id,
        DefaultIdType employeeId,
        DefaultIdType reviewerId,
        DateTime reviewPeriodStart,
        DateTime reviewPeriodEnd,
        string reviewType)
    {
        Id = id;
        EmployeeId = employeeId;
        ReviewerId = reviewerId;
        ReviewPeriodStart = reviewPeriodStart;
        ReviewPeriodEnd = reviewPeriodEnd;
        ReviewType = reviewType;
        Status = ReviewStatus.Draft;
        OverallRating = 0;

        QueueDomainEvent(new PerformanceReviewCreated { Review = this });
    }

    /// <summary>
    /// The employee being reviewed.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// The reviewer (manager/supervisor).
    /// </summary>
    public DefaultIdType ReviewerId { get; private set; }
    public Employee Reviewer { get; private set; } = default!;

    /// <summary>
    /// Review period start date.
    /// </summary>
    public DateTime ReviewPeriodStart { get; private set; }

    /// <summary>
    /// Review period end date.
    /// </summary>
    public DateTime ReviewPeriodEnd { get; private set; }

    /// <summary>
    /// Type of review (Annual, Quarterly, Probationary, etc.).
    /// </summary>
    public string ReviewType { get; private set; } = default!;

    /// <summary>
    /// Review status (Draft, Submitted, Completed, Acknowledged).
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Overall performance rating (1-5).
    /// </summary>
    public decimal OverallRating { get; private set; }

    /// <summary>
    /// Performance strengths.
    /// </summary>
    public string? Strengths { get; private set; }

    /// <summary>
    /// Areas for improvement.
    /// </summary>
    public string? AreasForImprovement { get; private set; }

    /// <summary>
    /// Goals for next period.
    /// </summary>
    public string? Goals { get; private set; }

    /// <summary>
    /// Reviewer comments.
    /// </summary>
    public string? ReviewerComments { get; private set; }

    /// <summary>
    /// Employee self-assessment comments.
    /// </summary>
    public string? EmployeeComments { get; private set; }

    /// <summary>
    /// Date review was submitted.
    /// </summary>
    public DateTime? SubmittedDate { get; private set; }

    /// <summary>
    /// Date review was completed.
    /// </summary>
    public DateTime? CompletedDate { get; private set; }

    /// <summary>
    /// Date employee acknowledged the review.
    /// </summary>
    public DateTime? AcknowledgedDate { get; private set; }

    /// <summary>
    /// Creates a new performance review.
    /// </summary>
    public static PerformanceReview Create(
        DefaultIdType employeeId,
        DefaultIdType reviewerId,
        DateTime reviewPeriodStart,
        DateTime reviewPeriodEnd,
        string reviewType)
    {
        if (reviewPeriodEnd <= reviewPeriodStart)
            throw new ArgumentException("Review period end must be after start date.");

        if (string.IsNullOrWhiteSpace(reviewType))
            throw new ArgumentException("Review type is required.", nameof(reviewType));

        var review = new PerformanceReview(
            DefaultIdType.NewGuid(),
            employeeId,
            reviewerId,
            reviewPeriodStart,
            reviewPeriodEnd,
            reviewType);

        return review;
    }

    /// <summary>
    /// Sets the overall rating.
    /// </summary>
    public PerformanceReview SetRating(decimal rating)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");

        OverallRating = rating;
        return this;
    }

    /// <summary>
    /// Sets performance strengths.
    /// </summary>
    public PerformanceReview SetStrengths(string? strengths)
    {
        Strengths = strengths;
        return this;
    }

    /// <summary>
    /// Sets areas for improvement.
    /// </summary>
    public PerformanceReview SetAreasForImprovement(string? areas)
    {
        AreasForImprovement = areas;
        return this;
    }

    /// <summary>
    /// Sets goals for next period.
    /// </summary>
    public PerformanceReview SetGoals(string? goals)
    {
        Goals = goals;
        return this;
    }

    /// <summary>
    /// Sets reviewer comments.
    /// </summary>
    public PerformanceReview SetReviewerComments(string? comments)
    {
        ReviewerComments = comments;
        return this;
    }

    /// <summary>
    /// Sets employee self-assessment comments.
    /// </summary>
    public PerformanceReview SetEmployeeComments(string? comments)
    {
        EmployeeComments = comments;
        return this;
    }

    /// <summary>
    /// Submits the review for completion.
    /// </summary>
    public PerformanceReview Submit()
    {
        if (Status != ReviewStatus.Draft)
            throw new InvalidOperationException("Only draft reviews can be submitted.");

        if (OverallRating == 0)
            throw new InvalidOperationException("Overall rating must be set before submitting.");

        Status = ReviewStatus.Submitted;
        SubmittedDate = DateTime.UtcNow;

        QueueDomainEvent(new PerformanceReviewSubmitted { Review = this });
        return this;
    }

    /// <summary>
    /// Completes the review.
    /// </summary>
    public PerformanceReview Complete()
    {
        if (Status != ReviewStatus.Submitted)
            throw new InvalidOperationException("Only submitted reviews can be completed.");

        Status = ReviewStatus.Completed;
        CompletedDate = DateTime.UtcNow;

        QueueDomainEvent(new PerformanceReviewCompleted { Review = this });
        return this;
    }

    /// <summary>
    /// Employee acknowledges the review.
    /// </summary>
    public PerformanceReview Acknowledge()
    {
        if (Status != ReviewStatus.Completed)
            throw new InvalidOperationException("Only completed reviews can be acknowledged.");

        Status = ReviewStatus.Acknowledged;
        AcknowledgedDate = DateTime.UtcNow;

        QueueDomainEvent(new PerformanceReviewAcknowledged { Review = this });
        return this;
    }

    /// <summary>
    /// Reopen review for editing.
    /// </summary>
    public PerformanceReview Reopen()
    {
        if (Status == ReviewStatus.Acknowledged)
            throw new InvalidOperationException("Cannot reopen acknowledged reviews.");

        Status = ReviewStatus.Draft;
        SubmittedDate = null;
        CompletedDate = null;

        return this;
    }
}

/// <summary>
/// Review type constants.
/// </summary>
public static class ReviewType
{
    /// <summary>Annual performance review.</summary>
    public const string Annual = "Annual";

    /// <summary>Quarterly performance review.</summary>
    public const string Quarterly = "Quarterly";

    /// <summary>Mid-year performance review.</summary>
    public const string MidYear = "MidYear";

    /// <summary>Probationary period review.</summary>
    public const string Probationary = "Probationary";

    /// <summary>Project completion review.</summary>
    public const string ProjectBased = "ProjectBased";

    /// <summary>360-degree feedback review.</summary>
    public const string ThreeSixty = "ThreeSixty";
}

/// <summary>
/// Review status constants.
/// </summary>
public static class ReviewStatus
{
    /// <summary>Draft review, not yet submitted.</summary>
    public const string Draft = "Draft";

    /// <summary>Submitted by reviewer.</summary>
    public const string Submitted = "Submitted";

    /// <summary>Completed and finalized.</summary>
    public const string Completed = "Completed";

    /// <summary>Acknowledged by employee.</summary>
    public const string Acknowledged = "Acknowledged";
}

