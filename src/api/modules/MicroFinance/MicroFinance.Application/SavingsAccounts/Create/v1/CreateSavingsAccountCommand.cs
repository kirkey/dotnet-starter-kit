using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;

public sealed record CreateSavingsAccountCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid MemberId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid SavingsProductId,
    [property: DefaultValue("USD")] string Currency,
    [property: DefaultValue(100)] decimal InitialDeposit) : IRequest<CreateSavingsAccountResponse>;
