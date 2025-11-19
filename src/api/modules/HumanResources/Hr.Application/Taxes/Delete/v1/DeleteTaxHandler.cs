using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;

/// <summary>
/// Handler for DeleteTaxCommand.
/// Deletes a tax master configuration from the database.
/// </summary>
public sealed class DeleteTaxHandler(
    ILogger<DeleteTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IRepository<TaxMaster> repository)
    : IRequestHandler<DeleteTaxCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the delete tax command.
    /// </summary>
    /// <param name="request">Delete tax command with tax ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the deleted tax.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="NotFoundException">Thrown when tax is not found.</exception>
    public async Task<DefaultIdType> Handle(DeleteTaxCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tax = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Tax with ID {request.Id} not found");

        await repository.DeleteAsync(tax, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Tax master deleted with ID {TaxId}",
            tax.Id);

        return tax.Id;
    }
}

