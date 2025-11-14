using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;

public sealed record UpdatePayrollDeductionCommand(
    DefaultIdType Id,
    [property: DefaultValue(0.0)] decimal? DeductionAmount = null,
    [property: DefaultValue(0.0)] decimal? DeductionPercentage = null,
    [property: DefaultValue(true)] bool? IsAuthorized = null,
    [property: DefaultValue(false)] bool? IsRecoverable = null,
    [property: DefaultValue("2025-12-31")] DateTime? EndDate = null,
    [property: DefaultValue(0.0)] decimal? MaxDeductionLimit = null,
    [property: DefaultValue("Updated remarks")] string? Remarks = null)
    : IRequest<UpdatePayrollDeductionResponse>;

