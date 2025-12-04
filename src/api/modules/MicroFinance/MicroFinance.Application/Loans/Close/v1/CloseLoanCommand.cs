using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Close.v1;

public sealed record CloseLoanCommand(Guid Id) : IRequest<CloseLoanResponse>;
