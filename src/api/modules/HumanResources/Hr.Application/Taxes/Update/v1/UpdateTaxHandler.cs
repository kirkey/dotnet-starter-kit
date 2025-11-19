using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Handler for UpdateTaxCommand.
/// Updates an existing tax master configuration in the database.
/// </summary>
public sealed class UpdateTaxHandler(
    ILogger<UpdateTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IRepository<TaxMaster> repository)
    : IRequestHandler<UpdateTaxCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the update tax command.
    /// </summary>
    /// <param name="request">Update tax command with configuration details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the updated tax.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    /// <exception cref="NotFoundException">Thrown when tax is not found.</exception>
    public async Task<DefaultIdType> Handle(UpdateTaxCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tax = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Tax with ID {request.Id} not found");

        tax.Update(
            name: request.Name,
            taxType: request.TaxType,
            rate: request.Rate,
            isCompound: request.IsCompound,
            jurisdiction: request.Jurisdiction,
            expiryDate: request.ExpiryDate,
            taxPaidAccountId: request.TaxPaidAccountId,
            taxAuthority: request.TaxAuthority,
            taxRegistrationNumber: request.TaxRegistrationNumber,
            reportingCategory: request.ReportingCategory);

        await repository.UpdateAsync(tax, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Tax master updated with ID {TaxId}",
            tax.Id);

        return tax.Id;
    }
}

