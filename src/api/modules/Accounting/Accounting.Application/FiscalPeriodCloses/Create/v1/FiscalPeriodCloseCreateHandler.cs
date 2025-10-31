using Accounting.Application.FiscalPeriodCloses.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.FiscalPeriodCloses.Create.v1;

/// <summary>
/// Handler for creating a new fiscal period close.
/// </summary>
public sealed class FiscalPeriodCloseCreateHandler(
    ILogger<FiscalPeriodCloseCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<FiscalPeriodClose> repository)
    : IRequestHandler<FiscalPeriodCloseCreateCommand, FiscalPeriodCloseCreateResponse>
{
    public async Task<FiscalPeriodCloseCreateResponse> Handle(FiscalPeriodCloseCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate close number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new FiscalPeriodCloseByNumberSpec(request.CloseNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicateFiscalPeriodCloseNumberException(request.CloseNumber);
        }

        // Check if period already has an active close
        var existingByPeriod = await repository.FirstOrDefaultAsync(
            new FiscalPeriodCloseByPeriodSpec(request.PeriodId), cancellationToken);
        if (existingByPeriod != null && existingByPeriod.Status != "Completed")
        {
            throw new PeriodCloseAlreadyCompletedException(existingByPeriod.Id);
        }

        var fiscalPeriodClose = FiscalPeriodClose.Create(
            closeNumber: request.CloseNumber,
            periodId: request.PeriodId,
            closeType: request.CloseType,
            periodStartDate: request.PeriodStartDate,
            periodEndDate: request.PeriodEndDate,
            initiatedBy: request.InitiatedBy,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(fiscalPeriodClose, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Fiscal period close created {CloseId} - {CloseNumber}", 
            fiscalPeriodClose.Id, fiscalPeriodClose.CloseNumber);
        return new FiscalPeriodCloseCreateResponse(fiscalPeriodClose.Id);
    }
}

