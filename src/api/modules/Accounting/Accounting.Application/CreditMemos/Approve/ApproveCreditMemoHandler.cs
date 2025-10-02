namespace Accounting.Application.CreditMemos.Approve;

/// <summary>
/// Command to approve a credit memo.
/// </summary>
public sealed record ApproveCreditMemoCommand(
    DefaultIdType Id,
    string ApprovedBy
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for approving a credit memo.
/// </summary>
public sealed class ApproveCreditMemoHandler(
    ILogger<ApproveCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IRepository<CreditMemo> repository)
    : IRequestHandler<ApproveCreditMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (creditMemo == null)
        {
            throw new CreditMemoNotFoundException(request.Id);
        }

        creditMemo.Approve(request.ApprovedBy);

        await repository.UpdateAsync(creditMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Credit memo {CreditMemoId} approved by {ApprovedBy}", 
            request.Id, request.ApprovedBy);

        return creditMemo.Id;
    }
}
