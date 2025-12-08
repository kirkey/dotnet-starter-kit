using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Create.v1;

public sealed record CreateShareAccountCommand(
    [property: DefaultValue("SH-000001")] string AccountNumber,
    DefaultIdType MemberId,
    DefaultIdType ShareProductId,
    [property: DefaultValue(null)] DateOnly? OpenedDate,
    [property: DefaultValue("Share account for member equity")] string? Notes) : IRequest<CreateShareAccountResponse>;
