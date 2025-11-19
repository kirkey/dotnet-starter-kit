namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;

/// <summary>
/// Request to get a benefit by Id.
/// </summary>
public sealed record GetBenefitRequest(
    DefaultIdType Id) : IRequest<BenefitResponse>;

