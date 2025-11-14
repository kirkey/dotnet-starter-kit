namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Command to create a new deduction.
/// </summary>
public sealed record CreateDeductionCommand(
    [property: DefaultValue("Health Insurance")] string ComponentName,
    [property: DefaultValue("Deduction")] string ComponentType = "Deduction",
    [property: DefaultValue("")] string GlAccountCode = "",
    [property: DefaultValue(null)] string? Description = null) : IRequest<CreateDeductionResponse>;

