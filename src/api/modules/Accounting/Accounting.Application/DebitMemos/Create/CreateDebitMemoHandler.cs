using Accounting.Domain.Entities;

namespace Accounting.Application.DebitMemos.Create;

/// <summary>
/// Handler for creating a debit memo.
/// </summary>
public sealed class CreateDebitMemoHandler(
    ILogger<CreateDebitMemoHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<CreateDebitMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateDebitMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var debitMemo = DebitMemo.Create(
            request.MemoNumber,
            request.MemoDate,
            request.Amount,
            request.ReferenceType,
            request.ReferenceId,
            request.OriginalDocumentId,
            request.Reason,
            request.Description,
            request.Notes);

        await repository.AddAsync(debitMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Debit memo {MemoNumber} created with ID {DebitMemoId}", 
            request.MemoNumber, debitMemo.Id);

        return debitMemo.Id;
    }
}
