namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Command to update a deduction.
/// </summary>
public sealed record UpdateDeductionCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? ComponentName = null,
    [property: DefaultValue(null)] string? GlAccountCode = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<UpdateDeductionResponse>;

