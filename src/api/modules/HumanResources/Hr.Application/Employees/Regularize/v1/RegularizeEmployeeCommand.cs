namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Regularize.v1;

/// <summary>
/// Command to regularize a probationary employee per Philippines Labor Code.
/// Typically after 6 months probation for general employees, 12 months for technical.
/// </summary>
public sealed record RegularizeEmployeeCommand(
    DefaultIdType Id,
    [property: DefaultValue("2025-06-01")] DateTime RegularizationDate
) : IRequest<RegularizeEmployeeResponse>;

/// <summary>
/// Response for employee regularization.
/// </summary>
public sealed record RegularizeEmployeeResponse(DefaultIdType Id, DateTime RegularizationDate);

