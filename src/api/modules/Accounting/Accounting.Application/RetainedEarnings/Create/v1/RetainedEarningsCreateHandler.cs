using Accounting.Application.RetainedEarnings.Queries;

namespace Accounting.Application.RetainedEarnings.Create.v1;

/// <summary>
/// Handler for creating a new retained earnings record.
/// </summary>
public sealed class RetainedEarningsCreateHandler(
    ILogger<RetainedEarningsCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Accounting.Domain.Entities.RetainedEarnings> repository)
    : IRequestHandler<RetainedEarningsCreateCommand, RetainedEarningsCreateResponse>
{
    public async Task<RetainedEarningsCreateResponse> Handle(RetainedEarningsCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate fiscal year
        var existingByYear = await repository.FirstOrDefaultAsync(
            new RetainedEarningsByFiscalYearSpec(request.FiscalYear), cancellationToken);
        if (existingByYear != null)
        {
            throw new DuplicateFiscalYearException(request.FiscalYear);
        }

        var retainedEarnings = Accounting.Domain.Entities.RetainedEarnings.Create(
            fiscalYear: request.FiscalYear,
            openingBalance: request.OpeningBalance,
            fiscalYearStartDate: request.FiscalYearStartDate,
            fiscalYearEndDate: request.FiscalYearEndDate,
            retainedEarningsAccountId: request.RetainedEarningsAccountId,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(retainedEarnings, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Retained earnings created {RetainedEarningsId} for FY{FiscalYear}", 
            retainedEarnings.Id, retainedEarnings.FiscalYear);
        return new RetainedEarningsCreateResponse(retainedEarnings.Id);
    }
}

