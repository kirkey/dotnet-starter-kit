using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Update.v1;

public sealed record UpdateShareProductCommand(
    Guid Id,
    [property: DefaultValue("Updated Common Shares")] string? Name,
    [property: DefaultValue("Updated description")] string? Description,
    [property: DefaultValue(110)] decimal? CurrentPrice,
    [property: DefaultValue(2000)] int? MaxSharesPerMember,
    [property: DefaultValue(true)] bool? AllowTransfer,
    [property: DefaultValue(true)] bool? AllowRedemption,
    [property: DefaultValue(6)] int? MinHoldingPeriodMonths,
    [property: DefaultValue(true)] bool? PaysDividends) : IRequest<UpdateShareProductResponse>;
