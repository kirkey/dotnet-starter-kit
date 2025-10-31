using Accounting.Application.PrepaidExpenses.Queries;
using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Application.PrepaidExpenses.Search.v1;

/// <summary>
/// Handler for searching prepaid expenses with filters.
/// </summary>
public sealed class SearchPrepaidExpensesHandler(
    ILogger<SearchPrepaidExpensesHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<PrepaidExpense> repository)
    : IRequestHandler<SearchPrepaidExpensesRequest, List<PrepaidExpenseResponse>>
{
    public async Task<List<PrepaidExpenseResponse>> Handle(SearchPrepaidExpensesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PrepaidExpenseSearchSpec(request.PrepaidNumber, request.Status);
        var expenses = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} prepaid expenses", expenses.Count);

        return expenses.Select(e => new PrepaidExpenseResponse
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
    }
}

