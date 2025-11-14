namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;

/// <summary>
/// Request to get a deduction by its identifier.
/// </summary>
public sealed record GetDeductionRequest(DefaultIdType Id) : IRequest<DeductionResponse>;

