using Accounting.Application.FiscalPeriodCloses.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Handler for completing a task in the fiscal period close process.
/// </summary>
public sealed class CompleteTaskHandler(
    ILogger<CompleteTaskHandler> logger,
    [FromKeyedServices("accounting")] IRepository<FiscalPeriodClose> repository)
    : IRequestHandler<CompleteTaskCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fiscalPeriodClose = await repository.FirstOrDefaultAsync(
            new FiscalPeriodCloseByIdSpec(request.FiscalPeriodCloseId), cancellationToken);

        if (fiscalPeriodClose == null)
        {
            throw new FiscalPeriodCloseByIdNotFoundException(request.FiscalPeriodCloseId);
        }

        fiscalPeriodClose.CompleteTask(request.TaskName);

        await repository.UpdateAsync(fiscalPeriodClose, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Task completed for fiscal period close {CloseId}: {TaskName}", 
            fiscalPeriodClose.Id, request.TaskName);
        return fiscalPeriodClose.Id;
    }
}

