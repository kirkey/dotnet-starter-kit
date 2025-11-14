namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Handler for creating a tax bracket.
/// </summary>
public sealed class CreateTaxHandler(
    ILogger<CreateTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IRepository<TaxBracket> repository)
    : IRequestHandler<CreateTaxCommand, CreateTaxResponse>
{
    public async Task<CreateTaxResponse> Handle(
        CreateTaxCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tax = TaxBracket.Create(
            request.TaxType,
            request.Year,
            request.MinIncome,
            request.MaxIncome,
            request.Rate);

        if (!string.IsNullOrWhiteSpace(request.FilingStatus) || !string.IsNullOrWhiteSpace(request.Description))
            tax.Update(
                filingStatus: request.FilingStatus,
                description: request.Description);

        await repository.AddAsync(tax, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Tax bracket created with ID {TaxId}, Type {TaxType}, Year {Year}, Rate {Rate}",
            tax.Id,
            request.TaxType,
            request.Year,
            request.Rate);

        return new CreateTaxResponse(tax.Id);
    }
}

