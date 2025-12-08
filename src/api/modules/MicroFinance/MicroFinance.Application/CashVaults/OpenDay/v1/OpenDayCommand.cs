using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.OpenDay.v1;

public sealed record OpenDayCommand(DefaultIdType Id, decimal VerifiedBalance) : IRequest<OpenDayResponse>;
