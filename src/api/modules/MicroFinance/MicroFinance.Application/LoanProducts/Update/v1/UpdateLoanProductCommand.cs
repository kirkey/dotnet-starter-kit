using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Update.v1;

public sealed record UpdateLoanProductCommand(
    Guid Id,
    [property: DefaultValue("Personal Loan Updated")] string? Name,
    [property: DefaultValue("Updated description")] string? Description,
    [property: DefaultValue(2000)] decimal? MinLoanAmount,
    [property: DefaultValue(150000)] decimal? MaxLoanAmount,
    [property: DefaultValue(15.0)] decimal? InterestRate,
    [property: DefaultValue("Declining")] string? InterestMethod,
    [property: DefaultValue(1)] int? MinTermMonths,
    [property: DefaultValue(72)] int? MaxTermMonths,
    [property: DefaultValue("Monthly")] string? RepaymentFrequency,
    [property: DefaultValue(7)] int? GracePeriodDays,
    [property: DefaultValue(2.5)] decimal? LatePenaltyRate) : IRequest<UpdateLoanProductResponse>;
