namespace Accounting.Application.DebitMemos.Apply;

/// <summary>
/// Command to apply a debit memo to an invoice or bill.
/// </summary>
public sealed record ApplyDebitMemoCommand(
    DefaultIdType Id,
    decimal AmountToApply,
    DefaultIdType TargetDocumentId
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for applying a debit memo.
/// </summary>
public sealed class ApplyDebitMemoHandler(
    ILogger<ApplyDebitMemoHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<ApplyDebitMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApplyDebitMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var debitMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (debitMemo == null)
        {
            throw new DebitMemoNotFoundException(request.Id);
        }

        debitMemo.Apply(request.AmountToApply, request.TargetDocumentId);

        await repository.UpdateAsync(debitMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Debit memo {DebitMemoId} applied {Amount} to document {DocumentId}", 
            request.Id, request.AmountToApply, request.TargetDocumentId);

        return debitMemo.Id;
    }
}
