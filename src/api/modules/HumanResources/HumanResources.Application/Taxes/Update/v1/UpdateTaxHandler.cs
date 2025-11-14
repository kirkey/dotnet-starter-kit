namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Handler for updating a tax bracket.
/// </summary>
public sealed class UpdateTaxHandler(
    ILogger<UpdateTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IRepository<TaxBracket> repository)
    : IRequestHandler<UpdateTaxCommand, UpdateTaxResponse>
{
    public async Task<UpdateTaxResponse> Handle(
        UpdateTaxCommand request,
        CancellationToken cancellationToken)
    {
        var tax = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (tax is null)
            throw new Exception($"Tax bracket not found: {request.Id}");

        tax.Update(
            filingStatus: request.FilingStatus,
            description: request.Description);

        await repository.UpdateAsync(tax, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Tax bracket {TaxId} updated successfully", tax.Id);

        return new UpdateTaxResponse(tax.Id);
    }
}

