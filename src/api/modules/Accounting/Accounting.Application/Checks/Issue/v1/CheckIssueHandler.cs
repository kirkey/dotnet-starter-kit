using Accounting.Application.Checks.Exceptions;
using Accounting.Application.Checks.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Issue.v1;

/// <summary>
/// Handler for issuing a check for payment.
/// </summary>
public sealed class CheckIssueHandler(
    ILogger<CheckIssueHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> repository)
    : IRequestHandler<CheckIssueCommand, CheckIssueResponse>
{
    public async Task<CheckIssueResponse> Handle(CheckIssueCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var check = await repository.GetByIdAsync(request.CheckId, cancellationToken);
        if (check == null)
        {
            throw new CheckNotFoundException(request.CheckId);
        }

        check.Issue(
            request.Amount,
            request.PayeeName,
            request.IssuedDate,
            request.PayeeId,
            request.VendorId,
            request.PaymentId,
            request.ExpenseId,
            request.Memo);

        await repository.UpdateAsync(check, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Check issued: {CheckId} - {CheckNumber} for {Amount} to {PayeeName}", 
            check.Id, check.CheckNumber, request.Amount, request.PayeeName);

        return new CheckIssueResponse(check.Id, check.CheckNumber, check.Status);
    }
}
