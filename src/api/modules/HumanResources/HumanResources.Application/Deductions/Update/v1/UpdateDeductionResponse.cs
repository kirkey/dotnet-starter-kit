namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Response for updating a deduction.
/// </summary>
/// <param name="Id">The identifier of the updated deduction.</param>
public sealed record UpdateDeductionResponse(DefaultIdType Id);

