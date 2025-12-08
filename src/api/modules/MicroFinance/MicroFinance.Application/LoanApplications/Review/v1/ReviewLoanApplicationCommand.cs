using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Review.v1;

/// <summary>
/// Command to complete review of a loan application.
/// </summary>
public sealed record ReviewLoanApplicationCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("Application documents verified and credit checked")] string ReviewNotes
) : IRequest<ReviewLoanApplicationResponse>;
