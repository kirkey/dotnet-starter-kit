using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.WriteOff.v1;

public sealed record WriteOffLoanCommand(
    Guid Id,
    [property: DefaultValue("Non-performing asset, exceeded 180 days overdue")] string WriteOffReason) : IRequest<WriteOffLoanResponse>;
