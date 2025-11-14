namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Handler for creating tax bracket with Philippines Tax Law compliance (TRAIN Law - RA 10963).
/// </summary>
public sealed class CreateTaxBracketHandler(
    ILogger<CreateTaxBracketHandler> logger,
    [FromKeyedServices("hr:taxbrackets")] IRepository<TaxBracket> repository)
    : IRequestHandler<CreateTaxBracketCommand, CreateTaxBracketResponse>
{
    public async Task<CreateTaxBracketResponse> Handle(
        CreateTaxBracketCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bracket = TaxBracket.Create(
            request.TaxType,
            request.Year,
            request.MinIncome,
            request.MaxIncome,
            request.Rate);

        if (!string.IsNullOrWhiteSpace(request.FilingStatus) || !string.IsNullOrWhiteSpace(request.Description))
        {
            bracket.Update(request.FilingStatus, request.Description);
        }

        await repository.AddAsync(bracket, cancellationToken);

        logger.LogInformation(
            "Tax bracket created: ID {Id}, Type {Type}, Year {Year}, Range {Min}-{Max}, Rate {Rate}%",
            bracket.Id,
            bracket.TaxType,
            bracket.Year,
            bracket.MinIncome,
            bracket.MaxIncome,
            bracket.Rate * 100);

        return new CreateTaxBracketResponse(
            bracket.Id,
            bracket.TaxType,
            bracket.Year,
            bracket.MinIncome,
            bracket.MaxIncome,
            bracket.Rate);
    }
}

