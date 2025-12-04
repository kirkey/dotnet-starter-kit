using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Delete.v1;

public sealed record DeleteLoanProductCommand(Guid Id) : IRequest<Unit>;
