using Accounting.Application.FiscalPeriodCloses.Queries;
using Accounting.Application.FiscalPeriodCloses.Responses;

namespace Accounting.Application.FiscalPeriodCloses.Get;

/// <summary>
/// Handler for retrieving a fiscal period close by its ID.
/// </summary>
public sealed class GetFiscalPeriodCloseHandler(
    ILogger<GetFiscalPeriodCloseHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<FiscalPeriodClose> repository)
    : IRequestHandler<GetFiscalPeriodCloseRequest, FiscalPeriodCloseResponse>
{
    public async Task<FiscalPeriodCloseResponse> Handle(GetFiscalPeriodCloseRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var close = await repository.FirstOrDefaultAsync(
            new FiscalPeriodCloseByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (close == null)
        {
            throw new NotFoundException($"Fiscal period close with ID {request.Id} was not found.");
        }

        logger.LogInformation("Retrieved fiscal period close {FiscalPeriodCloseId}", close.Id);

        return new FiscalPeriodCloseResponse
        {
            Id = close.Id,
            CloseNumber = close.CloseNumber,
            PeriodStartDate = close.PeriodStartDate,
            PeriodEndDate = close.PeriodEndDate,
            CloseDate = close.CompletedDate,
            Status = close.Status,
            CloseType = close.CloseType,
            Description = close.Description,
            Notes = close.Notes
        };
    }
}

