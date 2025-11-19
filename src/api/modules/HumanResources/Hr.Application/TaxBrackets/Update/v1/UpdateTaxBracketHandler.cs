namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Update.v1;

public sealed class UpdateTaxBracketHandler(
    ILogger<UpdateTaxBracketHandler> logger,
    [FromKeyedServices("hr:taxbrackets")] IRepository<TaxBracket> repository)
    : IRequestHandler<UpdateTaxBracketCommand, UpdateTaxBracketResponse>
{
    public async Task<UpdateTaxBracketResponse> Handle(UpdateTaxBracketCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bracket = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = bracket ?? throw new TaxBracketNotFoundException(request.Id);

        bracket.Update(
            filingStatus: request.FilingStatus,
            description: request.Description);

        await repository.UpdateAsync(bracket, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Tax bracket with id {BracketId} updated.", bracket.Id);

        return new UpdateTaxBracketResponse(bracket.Id);
    }
}

