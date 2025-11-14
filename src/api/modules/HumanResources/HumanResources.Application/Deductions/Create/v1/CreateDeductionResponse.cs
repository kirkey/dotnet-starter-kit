namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Response for creating a deduction.
/// </summary>
/// <param name="Id">The identifier of the created deduction.</param>
public sealed record CreateDeductionResponse(DefaultIdType Id);

