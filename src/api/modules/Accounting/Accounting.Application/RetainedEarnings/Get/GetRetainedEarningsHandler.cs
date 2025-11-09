using Accounting.Application.RetainedEarnings.Queries;
using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Get;

/// <summary>
/// Handler for retrieving detailed retained earnings information by ID.
/// </summary>
public sealed class GetRetainedEarningsHandler(
    ILogger<GetRetainedEarningsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<Domain.Entities.RetainedEarnings> repository)
    : IRequestHandler<GetRetainedEarningsRequest, RetainedEarningsDetailsResponse>
{
    public async Task<RetainedEarningsDetailsResponse> Handle(GetRetainedEarningsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var retainedEarnings = await repository.FirstOrDefaultAsync(
            new RetainedEarningsByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (retainedEarnings == null)
        {
            throw new NotFoundException($"Retained earnings with ID {request.Id} was not found.");
        }

        logger.LogInformation("Retrieved retained earnings {RetainedEarningsId}", retainedEarnings.Id);

        return new RetainedEarningsDetailsResponse
        {
            Id = retainedEarnings.Id,
            FiscalYear = retainedEarnings.FiscalYear,
            BeginningBalance = retainedEarnings.OpeningBalance,
            NetIncome = retainedEarnings.NetIncome,
            Dividends = retainedEarnings.Distributions,
            EndingBalance = retainedEarnings.ClosingBalance,
            Status = retainedEarnings.Status,
            IsClosed = retainedEarnings.IsClosed,
            Description = retainedEarnings.Description,
            CapitalContributions = retainedEarnings.CapitalContributions,
            OtherEquityChanges = retainedEarnings.OtherEquityChanges,
            ApproprietedAmount = retainedEarnings.ApproprietedAmount,
            UnappropriatedAmount = retainedEarnings.UnappropriatedAmount,
            FiscalYearStartDate = retainedEarnings.FiscalYearStartDate,
            FiscalYearEndDate = retainedEarnings.FiscalYearEndDate,
            ClosedDate = retainedEarnings.ClosedDate,
            ClosedBy = retainedEarnings.ClosedBy,
            RetainedEarningsAccountId = retainedEarnings.RetainedEarningsAccountId,
            DistributionCount = retainedEarnings.DistributionCount,
            LastDistributionDate = retainedEarnings.LastDistributionDate,
            Notes = retainedEarnings.Notes
        };
    }
}

