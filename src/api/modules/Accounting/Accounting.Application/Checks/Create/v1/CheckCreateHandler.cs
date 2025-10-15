using Accounting.Application.Checks.Exceptions;
using Accounting.Application.Checks.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Create.v1;

/// <summary>
/// Handler for creating a new check registration.
/// </summary>
public sealed class CheckCreateHandler(
    ILogger<CheckCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> repository)
    : IRequestHandler<CheckCreateCommand, CheckCreateResponse>
{
    public async Task<CheckCreateResponse> Handle(CheckCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate check number within the same bank account
        var existingCheck = await repository.FirstOrDefaultAsync(
            new CheckByNumberAndAccountSpec(request.CheckNumber, request.BankAccountCode), 
            cancellationToken);

        if (existingCheck != null)
        {
            throw new CheckNumberAlreadyExistsException(request.CheckNumber, request.BankAccountCode);
        }

        var entity = Check.Create(
            request.CheckNumber,
            request.BankAccountCode,
            request.BankAccountName,
            request.Description,
            request.Notes);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Check registered: {CheckId} - {CheckNumber}", entity.Id, entity.CheckNumber);
        return new CheckCreateResponse(entity.Id);
    }
}

