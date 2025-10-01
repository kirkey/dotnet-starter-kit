namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Update.v1;

public sealed record UpdateWholesaleContractCommand(
    DefaultIdType Id,
    DateTime? StartDate,
    DateTime? EndDate,
    decimal? MinimumOrderValue,
    decimal? VolumeDiscountPercentage,
    int? PaymentTermsDays,
    decimal? CreditLimit,
    string? DeliveryTerms,
    string? ContractTerms,
    bool? AutoRenewal,
    string? Notes,
    string? Status
) : IRequest<UpdateWholesaleContractResponse>;

