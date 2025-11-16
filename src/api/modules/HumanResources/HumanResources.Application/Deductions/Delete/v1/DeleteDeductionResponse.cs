namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Delete.v1;

/// <summary>
/// Response for deleting a deduction.
/// </summary>
/// <param name="Id">The identifier of the deleted deduction.</param>
/// <param name="IsDeleted">Whether the deduction was successfully deleted.</param>
public sealed record DeleteDeductionResponse(DefaultIdType Id, bool IsDeleted);

