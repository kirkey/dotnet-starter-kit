using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Update.v1;

public sealed record UpdatePayComponentRateCommand(
    DefaultIdType Id,
    [property: DefaultValue(0.045)] decimal? EmployeeRate = null,
    [property: DefaultValue(0.095)] decimal? EmployerRate = null,
    [property: DefaultValue(0.01)] decimal? AdditionalEmployerRate = null,
    [property: DefaultValue(0.0)] decimal? EmployeeAmount = null,
    [property: DefaultValue(0.0)] decimal? EmployerAmount = null,
    [property: DefaultValue(0.15)] decimal? TaxRate = null,
    [property: DefaultValue(0.0)] decimal? BaseAmount = null,
    [property: DefaultValue(0.20)] decimal? ExcessRate = null,
    [property: DefaultValue("2025-12-31")] DateTime? EffectiveEndDate = null,
    [property: DefaultValue("Updated description")] string? Description = null)
    : IRequest<UpdatePayComponentRateResponse>;

