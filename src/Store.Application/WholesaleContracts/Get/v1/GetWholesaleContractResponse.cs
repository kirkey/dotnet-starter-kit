namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Get.v1;

public sealed record GetWholesaleContractResponse(
    DefaultIdType Id,
    string ContractNumber,
    DefaultIdType CustomerId,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    decimal MinimumOrderValue,
    decimal VolumeDiscountPercentage,
    int PaymentTermsDays,
    decimal CreditLimit,
    string? DeliveryTerms,
    string? ContractTerms,
    bool AutoRenewal,
    string? Notes,
    DateTimeOffset CreatedOn,
    DateTimeOffset LastModifiedOn);

