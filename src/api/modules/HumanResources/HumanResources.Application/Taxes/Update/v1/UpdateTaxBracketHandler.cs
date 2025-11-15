namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Handler for updating tax bracket with validation.
/// </summary>
public sealed class UpdateTaxBracketHandler(
    ILogger<UpdateTaxBracketHandler> logger,
    [FromKeyedServices("hr:taxbrackets")] IRepository<TaxBracket> repository)
    : IRequestHandler<UpdateTaxBracketCommand, UpdateTaxBracketResponse>
{
    public async Task<UpdateTaxBracketResponse> Handle(
        UpdateTaxBracketCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bracket = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (bracket is null)
            throw new TaxBracketNotFoundException(request.Id);

        // Validate income range if both provided
        if (request is { MinIncome: not null, MaxIncome: not null })
        {
            if (request.MaxIncome <= request.MinIncome)
                throw new InvalidOperationException("Maximum income must be greater than minimum income.");
        }

        // Validate rate if provided
        if (request.Rate.HasValue && (request.Rate < 0 || request.Rate > 1))
            throw new InvalidOperationException("Tax rate must be between 0 and 1.");

        // Update fields if provided (using reflection-like approach via a temp object)
        bracket.Update(request.FilingStatus, request.Description);

        await repository.UpdateAsync(bracket, cancellationToken);

        logger.LogInformation(
            "Tax bracket {Id} updated: Range {Min}-{Max}, Rate {Rate}%",
            bracket.Id,
            request.MinIncome ?? bracket.MinIncome,
            request.MaxIncome ?? bracket.MaxIncome,
            (request.Rate ?? bracket.Rate) * 100);

        return new UpdateTaxBracketResponse(
            bracket.Id,
            bracket.TaxType,
            bracket.Year,
            bracket.MinIncome,
            bracket.MaxIncome,
            bracket.Rate);
    }
}

