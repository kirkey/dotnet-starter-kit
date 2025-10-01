namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Create.v1;

public sealed record CreateWholesaleContractCommand(
    string ContractNumber,
    DefaultIdType CustomerId,
    DateTime StartDate,
    DateTime EndDate,
    decimal MinimumOrderValue,
    decimal VolumeDiscountPercentage,
    int PaymentTermsDays = 30,
    decimal CreditLimit = 0,
    string? DeliveryTerms = null,
    string? ContractTerms = null,
    bool AutoRenewal = false,
    [property: DefaultValue(null)] string? Notes = null
) : IRequest<CreateWholesaleContractResponse>;

