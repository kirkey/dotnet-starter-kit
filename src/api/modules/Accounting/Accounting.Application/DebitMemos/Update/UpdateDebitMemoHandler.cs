using Accounting.Domain.Exceptions;

namespace Accounting.Application.DebitMemos.Update;

/// <summary>
/// Handler for updating a debit memo.
/// </summary>
public sealed class UpdateDebitMemoHandler(
    ILogger<UpdateDebitMemoHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<UpdateDebitMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateDebitMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var debitMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (debitMemo == null)
        {
            throw new DebitMemoNotFoundException(request.Id);
        }

        debitMemo.Update(
            request.MemoDate,
            request.Amount,
            request.Reason,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(debitMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Debit memo {DebitMemoId} updated", request.Id);

        return debitMemo.Id;
    }
}
