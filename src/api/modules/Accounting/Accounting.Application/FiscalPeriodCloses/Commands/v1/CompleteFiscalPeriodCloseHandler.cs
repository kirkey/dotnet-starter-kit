using Accounting.Application.FiscalPeriodCloses.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Handler for completing the fiscal period close process.
/// </summary>
public sealed class CompleteFiscalPeriodCloseHandler(
    ILogger<CompleteFiscalPeriodCloseHandler> logger,
    [FromKeyedServices("accounting")] IRepository<FiscalPeriodClose> repository)
    : IRequestHandler<CompleteFiscalPeriodCloseCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CompleteFiscalPeriodCloseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fiscalPeriodClose = await repository.FirstOrDefaultAsync(
            new FiscalPeriodCloseByIdSpec(request.FiscalPeriodCloseId), cancellationToken);

        if (fiscalPeriodClose == null)
        {
            throw new FiscalPeriodCloseByIdNotFoundException(request.FiscalPeriodCloseId);
        }

        fiscalPeriodClose.Complete(request.CompletedBy);

        await repository.UpdateAsync(fiscalPeriodClose, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Fiscal period close completed {CloseId} by {CompletedBy}", 
            fiscalPeriodClose.Id, request.CompletedBy);
        return fiscalPeriodClose.Id;
    }
}

