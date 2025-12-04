using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Create.v1;

public sealed record CreateShareProductCommand(
    [property: DefaultValue("COMMON")] string Code,
    [property: DefaultValue("Common Shares")] string Name,
    [property: DefaultValue("Standard member shares with voting rights")] string? Description,
    [property: DefaultValue(100)] decimal NominalValue,
    [property: DefaultValue(100)] decimal CurrentPrice,
    [property: DefaultValue(1)] int MinSharesForMembership,
    [property: DefaultValue(1000)] int? MaxSharesPerMember,
    [property: DefaultValue(false)] bool AllowTransfer,
    [property: DefaultValue(true)] bool AllowRedemption,
    [property: DefaultValue(12)] int? MinHoldingPeriodMonths,
    [property: DefaultValue(true)] bool PaysDividends) : IRequest<CreateShareProductResponse>;
