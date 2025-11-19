namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Handler for CreateTaxCommand.
/// Persists new tax master configuration to the database.
/// </summary>
public sealed class CreateTaxHandler(
    ILogger<CreateTaxHandler> logger,
    [FromKeyedServices("hr:taxes")] IRepository<TaxMaster> repository)
    : IRequestHandler<CreateTaxCommand, CreateTaxResponse>
{
    /// <summary>
    /// Handles the create tax command.
    /// </summary>
    /// <param name="request">Create tax command with configuration details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response with the created tax ID.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    public async Task<CreateTaxResponse> Handle(CreateTaxCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tax = TaxMaster.Create(
            code: request.Code,
            name: request.Name,
            taxType: request.TaxType,
            rate: request.Rate,
            taxCollectedAccountId: request.TaxCollectedAccountId!.Value,
            effectiveDate: request.EffectiveDate,
            isCompound: request.IsCompound,
            jurisdiction: request.Jurisdiction,
            expiryDate: request.ExpiryDate,
            taxPaidAccountId: request.TaxPaidAccountId,
            taxAuthority: request.TaxAuthority,
            taxRegistrationNumber: request.TaxRegistrationNumber,
            reportingCategory: request.ReportingCategory);

        await repository.AddAsync(tax, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Tax master created with ID {TaxId}, Code {TaxCode}, Type {TaxType}, Rate {Rate}%",
            tax.Id,
            request.Code,
            request.TaxType,
            request.Rate * 100);

        return new CreateTaxResponse(tax.Id);
    }
}

