using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Update.v1;

public sealed record UpdateLoanCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(10.5)] decimal? InterestRate,
    [property: DefaultValue(12)] int? TermMonths,
    [property: DefaultValue("Working capital for business expansion")] string? Purpose,
    [property: DefaultValue("Monthly")] string? RepaymentFrequency) : IRequest<UpdateLoanResponse>;
