namespace Accounting.Application.CreditMemos.Create;

/// <summary>
/// Handler for creating a credit memo.
/// </summary>
public sealed class CreateCreditMemoHandler(
    ILogger<CreateCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IRepository<CreditMemo> repository)
    : IRequestHandler<CreateCreditMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = CreditMemo.Create(
            request.MemoNumber,
            request.MemoDate,
            request.Amount,
            request.ReferenceType,
            request.ReferenceId,
            request.OriginalDocumentId,
            request.Reason,
            request.Description,
            request.Notes);

        await repository.AddAsync(creditMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Credit memo {MemoNumber} created with ID {CreditMemoId}", 
            request.MemoNumber, creditMemo.Id);

        return creditMemo.Id;
    }
}
