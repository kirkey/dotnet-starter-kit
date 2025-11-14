namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;

/// <summary>
/// Handler for deleting tax bracket.
/// </summary>
public sealed class DeleteTaxBracketHandler(
    ILogger<DeleteTaxBracketHandler> logger,
    [FromKeyedServices("hr:taxbrackets")] IRepository<TaxBracket> repository)
    : IRequestHandler<DeleteTaxBracketCommand, DeleteTaxBracketResponse>
{
    public async Task<DeleteTaxBracketResponse> Handle(
        DeleteTaxBracketCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bracket = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (bracket is null)
            throw new TaxBracketNotFoundException(request.Id);

        await repository.DeleteAsync(bracket, cancellationToken);

        logger.LogInformation("Tax bracket {Id} deleted", bracket.Id);

        return new DeleteTaxBracketResponse(bracket.Id, true);
    }
}

