using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Create.v1;

public sealed record CreateLoanProductCommand(
    [property: DefaultValue("LP001")] string Code,
    [property: DefaultValue("Personal Loan")] string Name,
    [property: DefaultValue("Short-term personal loan for emergency needs")] string? Description,
    [property: DefaultValue(1000)] decimal MinLoanAmount,
    [property: DefaultValue(100000)] decimal MaxLoanAmount,
    [property: DefaultValue(12.5)] decimal InterestRate,
    [property: DefaultValue("Declining")] string InterestMethod,
    [property: DefaultValue(1)] int MinTermMonths,
    [property: DefaultValue(60)] int MaxTermMonths,
    [property: DefaultValue("Monthly")] string RepaymentFrequency,
    [property: DefaultValue(5)] int GracePeriodDays = 0,
    [property: DefaultValue(2.0)] decimal LatePenaltyRate = 0) : IRequest<CreateLoanProductResponse>;
