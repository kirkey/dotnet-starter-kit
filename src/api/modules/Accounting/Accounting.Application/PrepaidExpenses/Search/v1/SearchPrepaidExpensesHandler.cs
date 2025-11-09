using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Application.PrepaidExpenses.Search.v1;

/// <summary>
/// Handler for searching prepaid expenses with filters and pagination.
/// </summary>
public sealed class SearchPrepaidExpensesHandler(
    ILogger<SearchPrepaidExpensesHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<PrepaidExpense> repository)
    : IRequestHandler<SearchPrepaidExpensesRequest, PagedList<PrepaidExpenseResponse>>
{
    public async Task<PagedList<PrepaidExpenseResponse>> Handle(SearchPrepaidExpensesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPrepaidExpensesSpec(request);
        var expenses = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} of {Total} prepaid expenses", expenses.Count, totalCount);

        var responses = expenses.Select(e => new PrepaidExpenseResponse
        {
            Id = e.Id,
            PrepaidNumber = e.PrepaidNumber,
            TotalAmount = e.TotalAmount,
            AmortizedAmount = e.AmortizedAmount,
            RemainingAmount = e.RemainingAmount,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            AmortizationSchedule = e.AmortizationSchedule,
            Status = e.Status,
            PrepaidAssetAccountId = e.PrepaidAssetAccountId,
            ExpenseAccountId = e.ExpenseAccountId,
            VendorId = e.VendorId,
            VendorName = e.VendorName,
            IsFullyAmortized = e.IsFullyAmortized,
            Description = e.Description,
            Notes = e.Notes
        }).ToList();

        return new PagedList<PrepaidExpenseResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

