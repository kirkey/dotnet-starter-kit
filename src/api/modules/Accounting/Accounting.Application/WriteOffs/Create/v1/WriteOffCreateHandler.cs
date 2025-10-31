using Accounting.Application.WriteOffs.Queries;

namespace Accounting.Application.WriteOffs.Create.v1;

/// <summary>
/// Handler for creating a new write-off.
/// </summary>
public sealed class WriteOffCreateHandler(
    ILogger<WriteOffCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<WriteOff> repository)
    : IRequestHandler<WriteOffCreateCommand, WriteOffCreateResponse>
{
    public async Task<WriteOffCreateResponse> Handle(WriteOffCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate reference number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new WriteOffByReferenceNumberSpec(request.ReferenceNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicateWriteOffReferenceNumberException(request.ReferenceNumber);
        }

        // Parse write-off type enum
        if (!Enum.TryParse<WriteOffType>(request.WriteOffType, out var writeOffType))
        {
            throw new InvalidWriteOffTypeException(request.WriteOffType);
        }

        var writeOff = WriteOff.Create(
            referenceNumber: request.ReferenceNumber,
            writeOffDate: request.WriteOffDate,
            writeOffType: writeOffType,
            amount: request.Amount,
            receivableAccountId: request.ReceivableAccountId,
            expenseAccountId: request.ExpenseAccountId,
            customerId: request.CustomerId,
            customerName: request.CustomerName,
            invoiceId: request.InvoiceId,
            invoiceNumber: request.InvoiceNumber,
            reason: request.Reason,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(writeOff, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Write-off created {WriteOffId} - {ReferenceNumber}", 
            writeOff.Id, writeOff.ReferenceNumber);
        return new WriteOffCreateResponse(writeOff.Id);
    }
}

