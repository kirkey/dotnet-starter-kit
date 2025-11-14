using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Create.v1;

/// <summary>
/// Command to create a new pay component rate/bracket configuration.
/// </summary>
public sealed record CreatePayComponentRateCommand(
    [property: DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afa6")] DefaultIdType PayComponentId,
    [property: DefaultValue(4000.0)] decimal MinAmount,
    [property: DefaultValue(4250.0)] decimal MaxAmount,
    [property: DefaultValue(2025)] int Year,
    [property: DefaultValue(0.045)] decimal? EmployeeRate = null,
    [property: DefaultValue(0.095)] decimal? EmployerRate = null,
    [property: DefaultValue(0.01)] decimal? AdditionalEmployerRate = null,
    [property: DefaultValue(0.0)] decimal? EmployeeAmount = null,
    [property: DefaultValue(0.0)] decimal? EmployerAmount = null,
    [property: DefaultValue(0.15)] decimal? TaxRate = null,
    [property: DefaultValue(0.0)] decimal? BaseAmount = null,
    [property: DefaultValue(0.20)] decimal? ExcessRate = null,
    [property: DefaultValue("2025-01-01")] DateTime? EffectiveStartDate = null,
    [property: DefaultValue("2025-12-31")] DateTime? EffectiveEndDate = null,
    [property: DefaultValue("SSS bracket for 4000-4250 salary range")] string? Description = null)
    : IRequest<CreatePayComponentRateResponse>;

