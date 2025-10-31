namespace Accounting.Application.TaxCodes.Create.v1;

public sealed class CreateTaxCodeHandler(
    ILogger<CreateTaxCodeHandler> logger,
    IRepository<TaxCode> repository)
    : IRequestHandler<CreateTaxCodeCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateTaxCodeCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!Enum.TryParse<TaxType>(command.TaxType, true, out var taxType))
            throw new ArgumentException($"Invalid tax type: {command.TaxType}", nameof(command.TaxType));

        var taxCode = TaxCode.Create(
            code: command.Code,
            name: command.Name ?? command.Code,
            taxType: taxType,
            rate: command.Rate,
            taxCollectedAccountId: command.TaxCollectedAccountId,
            effectiveDate: command.EffectiveDate,
            isCompound: command.IsCompound,
            jurisdiction: command.Jurisdiction,
            expiryDate: command.ExpiryDate,
            taxPaidAccountId: command.TaxPaidAccountId,
            taxAuthority: command.TaxAuthority,
            taxRegistrationNumber: command.TaxRegistrationNumber,
            reportingCategory: command.ReportingCategory,
            description: command.Description);

        await repository.AddAsync(taxCode, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Tax code created {TaxCodeId}", taxCode.Id);

        return taxCode.Id;
    }
}
