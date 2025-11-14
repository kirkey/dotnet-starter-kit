namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Delete.v1;

public sealed class DeleteTaxBracketHandler(
    ILogger<DeleteTaxBracketHandler> logger,
    [FromKeyedServices("hr:taxbrackets")] IRepository<TaxBracket> repository)
    : IRequestHandler<DeleteTaxBracketCommand, DeleteTaxBracketResponse>
{
    public async Task<DeleteTaxBracketResponse> Handle(DeleteTaxBracketCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bracket = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = bracket ?? throw new TaxBracketNotFoundException(request.Id);

        await repository.DeleteAsync(bracket, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Tax bracket with id {BracketId} deleted.", bracket.Id);

        return new DeleteTaxBracketResponse(bracket.Id);
    }
}

