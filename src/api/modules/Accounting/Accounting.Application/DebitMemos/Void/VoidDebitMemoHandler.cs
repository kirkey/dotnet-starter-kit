using Accounting.Domain.Exceptions;

namespace Accounting.Application.DebitMemos.Void;

/// <summary>
/// Command to void a debit memo.
/// </summary>
public sealed record VoidDebitMemoCommand(
    DefaultIdType Id,
    string? VoidReason = null
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for voiding a debit memo.
/// </summary>
public sealed class VoidDebitMemoHandler(
    ILogger<VoidDebitMemoHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<VoidDebitMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(VoidDebitMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var debitMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (debitMemo == null)
        {
            throw new DebitMemoNotFoundException(request.Id);
        }

        debitMemo.Void(request.VoidReason);

        await repository.UpdateAsync(debitMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Debit memo {DebitMemoId} voided. Reason: {Reason}", 
            request.Id, request.VoidReason ?? "Not specified");

        return debitMemo.Id;
    }
}
