namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;

/// <summary>
/// Handler for deleting a tax bracket.
/// </summary>
public sealed class DeleteTaxHandler(
    ILogger<DeleteTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IRepository<TaxBracket> repository)
    : IRequestHandler<DeleteTaxCommand, DeleteTaxResponse>
{
    public async Task<DeleteTaxResponse> Handle(
        DeleteTaxCommand request,
        CancellationToken cancellationToken)
    {
        var tax = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (tax is null)
            throw new Exception($"Tax bracket not found: {request.Id}");

        await repository.DeleteAsync(tax, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Tax bracket {TaxId} deleted successfully", tax.Id);

        return new DeleteTaxResponse(tax.Id);
    }
}
