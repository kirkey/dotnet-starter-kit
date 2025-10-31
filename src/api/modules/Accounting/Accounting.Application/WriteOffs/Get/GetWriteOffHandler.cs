using Accounting.Application.WriteOffs.Queries;
using Accounting.Application.WriteOffs.Responses;

namespace Accounting.Application.WriteOffs.Get;

/// <summary>
/// Handler for retrieving a write-off by ID.
/// </summary>
public class GetWriteOffHandler(
    [FromKeyedServices("accounting")] IReadRepository<WriteOff> repository)
    : IRequestHandler<GetWriteOffRequest, WriteOffResponse>
{
    public async Task<WriteOffResponse> Handle(
        GetWriteOffRequest request,
        CancellationToken cancellationToken)
    {
        var writeOff = await repository.FirstOrDefaultAsync(
            new WriteOffByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (writeOff is null)
        {
            throw new NotFoundException(
                $"{nameof(WriteOff)} with ID {request.Id} was not found.");
        }

        return new WriteOffResponse
        {
            Id = writeOff.Id,
            ReferenceNumber = writeOff.ReferenceNumber,
            WriteOffDate = writeOff.WriteOffDate,
            WriteOffType = writeOff.WriteOffType.ToString(),
            Amount = writeOff.Amount,
            RecoveredAmount = writeOff.RecoveredAmount,
            IsRecovered = writeOff.IsRecovered,
            CustomerId = writeOff.CustomerId,
            CustomerName = writeOff.CustomerName,
            InvoiceId = writeOff.InvoiceId,
            InvoiceNumber = writeOff.InvoiceNumber,
            ReceivableAccountId = writeOff.ReceivableAccountId,
            ExpenseAccountId = writeOff.ExpenseAccountId,
            JournalEntryId = writeOff.JournalEntryId,
            Status = writeOff.Status.ToString(),
            ApprovalStatus = writeOff.ApprovalStatus.ToString(),
            ApprovedBy = writeOff.ApprovedBy,
            ApprovedDate = writeOff.ApprovedDate,
            Reason = writeOff.Reason,
            Description = writeOff.Description,
            Notes = writeOff.Notes
        };
    }
}

