using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Get.v1;

public sealed class GetLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> repository)
    : IRequestHandler<GetLoanRequest, LoanResponse>
{
    public async Task<LoanResponse> Handle(GetLoanRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loan = await repository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.Id} not found.");
        }

        return new LoanResponse(
            loan.Id,
            loan.MemberId,
            $"{loan.Member!.FirstName} {loan.Member.LastName}",
            loan.Member.MemberNumber,
            loan.LoanProductId,
            loan.LoanProduct!.Name,
            loan.LoanNumber,
            loan.PrincipalAmount,
            loan.InterestRate,
            loan.TermMonths,
            loan.RepaymentFrequency,
            loan.Purpose,
            loan.ApplicationDate,
            loan.ApprovalDate,
            loan.DisbursementDate,
            loan.ExpectedEndDate,
            loan.ActualEndDate,
            loan.OutstandingPrincipal,
            loan.OutstandingInterest,
            loan.TotalPaid,
            loan.Status,
            loan.RejectionReason,
            loan.LoanSchedules.OrderBy(s => s.InstallmentNumber).Select(s => new LoanScheduleDto(
                s.Id,
                s.InstallmentNumber,
                s.DueDate,
                s.PrincipalAmount,
                s.InterestAmount,
                s.TotalAmount,
                s.PaidAmount,
                s.IsPaid,
                s.PaidDate
            )).ToList(),
            loan.LoanRepayments.OrderByDescending(r => r.RepaymentDate).Select(r => new LoanRepaymentDto(
                r.Id,
                r.RepaymentDate,
                r.TotalAmount,
                r.PrincipalAmount,
                r.InterestAmount,
                r.PenaltyAmount,
                r.PaymentMethod,
                r.Notes
            )).ToList(),
            loan.LoanGuarantors.Select(g => new LoanGuarantorDto(
                g.Id,
                g.GuarantorMemberId,
                g.GuarantorMember?.FirstName + " " + g.GuarantorMember?.LastName,
                g.Relationship,
                g.GuaranteedAmount,
                g.Status
            )).ToList(),
            loan.LoanCollaterals.Select(c => new LoanCollateralDto(
                c.Id,
                c.CollateralType,
                c.Description,
                c.EstimatedValue,
                c.DocumentReference,
                c.Status
            )).ToList()
        );
    }
}
