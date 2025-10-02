using Accounting.Domain.Entities;

namespace Accounting.Application.TaxCodes.Delete.v1;

public sealed class DeleteTaxCodeHandler(
    ILogger<DeleteTaxCodeHandler> logger,
    IRepository<TaxCode> repository)
    : IRequestHandler<DeleteTaxCodeCommand>
{
    public async Task Handle(DeleteTaxCodeCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var taxCode = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (taxCode == null)
            throw new TaxCodeNotFoundException(command.Id);

        await repository.DeleteAsync(taxCode, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Tax code deleted {TaxCodeId}", command.Id);
    }
}
