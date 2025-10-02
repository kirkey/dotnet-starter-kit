using Accounting.Domain.Exceptions;

namespace Accounting.Application.DebitMemos.Approve;

/// <summary>
/// Command to approve a debit memo.
/// </summary>
public sealed record ApproveDebitMemoCommand(
    DefaultIdType Id,
    string ApprovedBy
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for approving a debit memo.
/// </summary>
public sealed class ApproveDebitMemoHandler(
    ILogger<ApproveDebitMemoHandler> logger,
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<ApproveDebitMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveDebitMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var debitMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (debitMemo == null)
        {
            throw new DebitMemoNotFoundException(request.Id);
        }

        debitMemo.Approve(request.ApprovedBy);

        await repository.UpdateAsync(debitMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Debit memo {DebitMemoId} approved by {ApprovedBy}", 
            request.Id, request.ApprovedBy);

        return debitMemo.Id;
    }
}
