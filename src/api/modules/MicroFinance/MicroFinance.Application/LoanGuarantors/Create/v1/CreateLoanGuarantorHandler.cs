using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Create.v1;

public sealed class CreateLoanGuarantorHandler(
    [FromKeyedServices("microfinance:loans")] IReadRepository<Loan> loanRepository,
    [FromKeyedServices("microfinance:members")] IReadRepository<Member> memberRepository,
    [FromKeyedServices("microfinance:loanguarantors")] IRepository<LoanGuarantor> guarantorRepository,
    ILogger<CreateLoanGuarantorHandler> logger)
    : IRequestHandler<CreateLoanGuarantorCommand, CreateLoanGuarantorResponse>
{
    public async Task<CreateLoanGuarantorResponse> Handle(CreateLoanGuarantorCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify loan exists
        var loan = await loanRepository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.LoanId), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.LoanId} not found.");
        }

        // Verify guarantor member exists
        var guarantorMember = await memberRepository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.GuarantorMemberId), cancellationToken).ConfigureAwait(false);

        if (guarantorMember is null)
        {
            throw new NotFoundException($"Guarantor member with ID {request.GuarantorMemberId} not found.");
        }

        // Verify guarantor is not the borrower
        if (loan.MemberId == request.GuarantorMemberId)
        {
            throw new InvalidOperationException("A member cannot guarantee their own loan.");
        }

        // Create the guarantor
        var guarantor = LoanGuarantor.Create(
            request.LoanId,
            request.GuarantorMemberId,
            request.GuaranteedAmount,
            request.Relationship,
            request.GuaranteeDate,
            request.ExpiryDate,
            request.Notes);

        await guarantorRepository.AddAsync(guarantor, cancellationToken).ConfigureAwait(false);
        await guarantorRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Created loan guarantor {GuarantorId} for loan {LoanId}. Member: {MemberId}, Amount: {Amount}",
            guarantor.Id, request.LoanId, request.GuarantorMemberId, request.GuaranteedAmount);

        return new CreateLoanGuarantorResponse(guarantor.Id);
    }
}
