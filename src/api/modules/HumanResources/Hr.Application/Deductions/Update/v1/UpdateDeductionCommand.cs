namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Command to update a deduction.
/// </summary>
public sealed record UpdateDeductionCommand(
    DefaultIdType Id,
    string? DeductionName = null,
    string? DeductionType = null,
    string? RecoveryMethod = null,
    decimal? RecoveryFixedAmount = null,
    decimal? RecoveryPercentage = null,
    int? InstallmentCount = null,
    decimal? MaxRecoveryPercentage = null,
    bool? RequiresApproval = null,
    bool? IsRecurring = null,
    bool? IsActive = null,
    string? GlAccountCode = null,
    string? Description = null) : IRequest<UpdateDeductionResponse>;
