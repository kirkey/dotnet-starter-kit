namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Update.v1;

public sealed record UpdateEmployeePayComponentCommand(
    DefaultIdType Id,
    [property: DefaultValue(2500.0)] decimal? CustomRate = null,
    [property: DefaultValue(6000.0)] decimal? FixedAmount = null,
    [property: DefaultValue("BasicPay * 0.12")] string? CustomFormula = null,
    [property: DefaultValue("Updated remarks")] string? Remarks = null)
    : IRequest<UpdateEmployeePayComponentResponse>;

