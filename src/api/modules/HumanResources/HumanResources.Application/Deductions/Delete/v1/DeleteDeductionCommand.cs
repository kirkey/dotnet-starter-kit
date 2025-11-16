namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Delete.v1;

/// <summary>
/// Command to delete a deduction.
/// </summary>
public sealed record DeleteDeductionCommand(
    DefaultIdType Id) : IRequest<DeleteDeductionResponse>;
