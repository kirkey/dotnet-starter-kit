namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Command to create a new deduction type per Philippines Labor Code Art 113.
/// Includes recovery rules, DOLE compliance, and approval requirements.
/// </summary>
public sealed record CreateDeductionCommand(
    [property: DefaultValue("SSS Loan")] string DeductionName,
    [property: DefaultValue("Loan")] string DeductionType,
    
    // Recovery Configuration
    [property: DefaultValue("Manual")] string RecoveryMethod = "Manual",
    [property: DefaultValue(null)] decimal? RecoveryFixedAmount = null,
    [property: DefaultValue(null)] decimal? RecoveryPercentage = null,
    [property: DefaultValue(null)] int? InstallmentCount = null,
    
    // Compliance & Rules
    [property: DefaultValue(20.00)] decimal MaxRecoveryPercentage = 20.00m,
    [property: DefaultValue(true)] bool RequiresApproval = true,
    [property: DefaultValue(false)] bool IsRecurring = false,
    
    // Accounting & Description
    [property: DefaultValue(null)] string? GlAccountCode = null,
    [property: DefaultValue(null)] string? Description = null
) : IRequest<CreateDeductionResponse>;
