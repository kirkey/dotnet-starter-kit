using Accounting.Application.FiscalPeriodCloses.Queries;

namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Handler for reopening a fiscal period close.
/// </summary>
public sealed class ReopenFiscalPeriodCloseHandler(
    ILogger<ReopenFiscalPeriodCloseHandler> logger,
    [FromKeyedServices("accounting")] IRepository<FiscalPeriodClose> repository)
    : IRequestHandler<ReopenFiscalPeriodCloseCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ReopenFiscalPeriodCloseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fiscalPeriodClose = await repository.FirstOrDefaultAsync(
            new FiscalPeriodCloseByIdSpec(request.FiscalPeriodCloseId), cancellationToken);

        if (fiscalPeriodClose == null)
        {
            throw new FiscalPeriodCloseByIdNotFoundException(request.FiscalPeriodCloseId);
        }

        fiscalPeriodClose.Reopen(request.ReopenedBy, request.Reason);

        await repository.UpdateAsync(fiscalPeriodClose, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Fiscal period close reopened {CloseId} by {ReopenedBy} - {Reason}", 
            fiscalPeriodClose.Id, request.ReopenedBy, request.Reason);
        return fiscalPeriodClose.Id;
    }
}

