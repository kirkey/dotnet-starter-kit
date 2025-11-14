using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Create.v1;

public sealed class CreateTaxBracketHandler(
    ILogger<CreateTaxBracketHandler> logger,
    [FromKeyedServices("hr:taxbrackets")] IRepository<TaxBracket> repository)
    : IRequestHandler<CreateTaxBracketCommand, CreateTaxBracketResponse>
{
    public async Task<CreateTaxBracketResponse> Handle(CreateTaxBracketCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bracket = TaxBracket.Create(
            request.TaxType,
            request.Year,
            request.MinIncome,
            request.MaxIncome,
            request.Rate);

        // Update optional properties
        if (!string.IsNullOrWhiteSpace(request.FilingStatus) || !string.IsNullOrWhiteSpace(request.Description))
        {
            bracket.Update(
                filingStatus: request.FilingStatus,
                description: request.Description);
        }

        await repository.AddAsync(bracket, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Tax bracket created {BracketId} for {TaxType} year {Year}, range {MinIncome}-{MaxIncome}, rate {Rate}%",
            bracket.Id,
            request.TaxType,
            request.Year,
            request.MinIncome,
            request.MaxIncome,
            request.Rate * 100);

        return new CreateTaxBracketResponse(bracket.Id);
    }
}
