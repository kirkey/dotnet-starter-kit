namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;

/// <summary>
/// Request to get benefit details.
/// </summary>
public sealed record GetBenefitRequest(DefaultIdType Id) : IRequest<BenefitResponse>;

