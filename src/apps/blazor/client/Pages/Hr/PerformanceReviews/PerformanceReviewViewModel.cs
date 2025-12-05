using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.PerformanceReviews;

/// <summary>
/// View model for creating and updating performance reviews.
/// </summary>
public class PerformanceReviewViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the performance review.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the employee being reviewed.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the selected employee (for autocomplete binding).
    /// </summary>
    public EmployeeResponse? SelectedEmployee { get; set; }

    /// <summary>
    /// Gets or sets the reviewer conducting the review.
    /// </summary>
    public DefaultIdType? ReviewerId { get; set; }

    /// <summary>
    /// Gets or sets the selected reviewer (for autocomplete binding).
    /// </summary>
    public EmployeeResponse? SelectedReviewer { get; set; }

    /// <summary>
    /// Gets or sets the start date of the review period.
    /// </summary>
    public DateTime? ReviewPeriodStart { get; set; }

    /// <summary>
    /// Gets or sets the end date of the review period.
    /// </summary>
    public DateTime? ReviewPeriodEnd { get; set; }

    /// <summary>
    /// Gets or sets the type of review (Annual, Quarterly, Probation, etc.).
    /// </summary>
    public string? ReviewType { get; set; } = "Annual";

    /// <summary>
    /// Gets or sets the overall performance rating (0-5).
    /// </summary>
    public decimal OverallRating { get; set; }

    /// <summary>
    /// Gets or sets the employee's strengths.
    /// </summary>
    public string? Strengths { get; set; }

    /// <summary>
    /// Gets or sets areas where the employee can improve.
    /// </summary>
    public string? AreasForImprovement { get; set; }

    /// <summary>
    /// Gets or sets goals for the next review period.
    /// </summary>
    public string? Goals { get; set; }

    /// <summary>
    /// Gets or sets comments from the reviewer.
    /// </summary>
    public string? ReviewerComments { get; set; }

    /// <summary>
    /// Gets or sets comments from the employee.
    /// </summary>
    public string? EmployeeComments { get; set; }

    /// <summary>
    /// Gets or sets the current status of the review.
    /// </summary>
    public string? Status { get; set; }
}
