namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

/// <summary>
/// Request for searching put-away tasks with filtering and pagination.
/// </summary>
public class SearchPutAwayTasksCommand : PaginationFilter, IRequest<PagedList<PutAwayTaskResponse>>
{
    /// <summary>
    /// Filter by put-away task status.
    /// </summary>
    public string? Status { get; init; }

    /// <summary>
    /// Filter by minimum priority level.
    /// </summary>
    public int? MinPriority { get; init; }

    /// <summary>
    /// Filter by maximum priority level.
    /// </summary>
    public int? MaxPriority { get; init; }

    /// <summary>
    /// Filter by warehouse identifier.
    /// </summary>
    public DefaultIdType? WarehouseId { get; init; }

    /// <summary>
    /// Filter by assigned worker.
    /// </summary>
    public string? AssignedTo { get; init; }

    /// <summary>
    /// Filter by task number (partial match).
    /// </summary>
    public string? TaskNumber { get; init; }

    /// <summary>
    /// Filter by creation date from.
    /// </summary>
    public DateTime? CreatedFrom { get; init; }

    /// <summary>
    /// Filter by creation date to.
    /// </summary>
    public DateTime? CreatedTo { get; init; }

    /// <summary>
    /// Filter by due date from.
    /// </summary>
    public DateTime? DueDateFrom { get; init; }

    /// <summary>
    /// Filter by due date to.
    /// </summary>
    public DateTime? DueDateTo { get; init; }

    /// <summary>
    /// Filter by goods receipt identifier.
    /// </summary>
    public DefaultIdType? GoodsReceiptId { get; init; }

    /// <summary>
    /// Filter by put-away strategy.
    /// </summary>
    public string? PutAwayStrategy { get; init; }

    /// <summary>
    /// Filter by start date from.
    /// </summary>
    public DateTime? StartDateFrom { get; init; }

    /// <summary>
    /// Filter by start date to.
    /// </summary>
    public DateTime? StartDateTo { get; init; }

    /// <summary>
    /// Filter by completed date from.
    /// </summary>
    public DateTime? CompletedDateFrom { get; init; }

    /// <summary>
    /// Filter by completed date to.
    /// </summary>
    public DateTime? CompletedDateTo { get; init; }

    /// <summary>
    /// Include completed tasks in results.
    /// </summary>
    public bool IncludeCompleted { get; init; } = true;

    /// <summary>
    /// Include cancelled tasks in results.
    /// </summary>
    public bool IncludeCancelled { get; init; } = false;
}
