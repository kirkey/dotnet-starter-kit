namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Terminate.v1;

/// <summary>
/// Command to terminate employee per Philippines Labor Code.
/// Includes termination reason, mode, and separation pay computation.
/// </summary>
public sealed record TerminateEmployeeCommand(
    DefaultIdType Id,
    [property: DefaultValue("2025-12-31")] DateTime TerminationDate,
    [property: DefaultValue("ResignationVoluntary")] string TerminationReason,
    [property: DefaultValue("ByEmployee")] string TerminationMode,
    [property: DefaultValue(null)] string? SeparationPayBasis = null,
    [property: DefaultValue(null)] decimal? SeparationPayAmount = null
) : IRequest<TerminateEmployeeResponse>;

/// <summary>
/// Response for employee termination.
/// </summary>
public sealed record TerminateEmployeeResponse(
    DefaultIdType Id,
    DateTime TerminationDate,
    decimal? SeparationPay);

