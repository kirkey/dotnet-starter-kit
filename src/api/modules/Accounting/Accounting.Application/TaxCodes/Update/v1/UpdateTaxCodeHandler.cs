namespace Accounting.Application.TaxCodes.Update.v1;

/// <summary>
/// Handler for updating a tax code's non-rate information.
/// Validates the tax code exists and updates allowed fields.
/// </summary>
public sealed class UpdateTaxCodeHandler(
    ILogger<UpdateTaxCodeHandler> logger,
    IRepository<TaxCode> repository)
    : IRequestHandler<UpdateTaxCodeCommand, UpdateTaxCodeResponse>
{
    /// <summary>
    /// Handles the update tax code request.
    /// </summary>
    /// <param name="command">The command containing update data.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The update response containing the tax code ID.</returns>
    /// <exception cref="TaxCodeNotFoundException">Thrown when tax code is not found.</exception>
    public async Task<UpdateTaxCodeResponse> Handle(UpdateTaxCodeCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var taxCode = await repository.GetByIdAsync(command.Id, cancellationToken).ConfigureAwait(false);
        _ = taxCode ?? throw new TaxCodeNotFoundException(command.Id);

        taxCode.Update(
            name: command.Name,
            jurisdiction: command.Jurisdiction,
            taxAuthority: command.TaxAuthority,
            taxRegistrationNumber: command.TaxRegistrationNumber,
            reportingCategory: command.ReportingCategory,
            description: command.Description);

        await repository.UpdateAsync(taxCode, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Tax code with id: {TaxCodeId} updated.", taxCode.Id);

        return new UpdateTaxCodeResponse(taxCode.Id);
    }
}

