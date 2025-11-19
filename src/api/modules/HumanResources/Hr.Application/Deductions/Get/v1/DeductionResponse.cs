namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;

/// <summary>
/// Response with deduction details.
/// </summary>
public sealed record DeductionResponse(
    DefaultIdType Id,
    string DeductionName,
    string DeductionType,
    string RecoveryMethod,
    decimal? RecoveryFixedAmount,
    decimal? RecoveryPercentage,
    int? InstallmentCount,
    decimal MaxRecoveryPercentage,
    bool RequiresApproval,
    bool IsRecurring,
    bool IsActive,
    string? GlAccountCode,
    string? Description);
