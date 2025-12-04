using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Create.v1;

public sealed class CreateLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> repository,
    [FromKeyedServices("microfinance:members")] IRepository<Member> memberRepository,
    [FromKeyedServices("microfinance:loanproducts")] IRepository<LoanProduct> loanProductRepository,
    ILogger<CreateLoanHandler> logger)
    : IRequestHandler<CreateLoanCommand, CreateLoanResponse>
{
    public async Task<CreateLoanResponse> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate member exists and is active
        var member = await memberRepository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.MemberId), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.MemberId} not found.");
        }

        if (!member.IsActive)
        {
            throw new InvalidOperationException("Cannot create loan for inactive member.");
        }

        // Validate loan product exists and is active
        var loanProduct = await loanProductRepository.FirstOrDefaultAsync(
            new LoanProductByIdSpec(request.LoanProductId), cancellationToken).ConfigureAwait(false);

        if (loanProduct is null)
        {
            throw new NotFoundException($"Loan product with ID {request.LoanProductId} not found.");
        }

        if (!loanProduct.IsActive)
        {
            throw new InvalidOperationException("Cannot create loan with inactive loan product.");
        }

        // Validate loan amount against product limits
        if (request.RequestedAmount < loanProduct.MinLoanAmount || request.RequestedAmount > loanProduct.MaxLoanAmount)
        {
            throw new InvalidOperationException(
                $"Requested amount must be between {loanProduct.MinLoanAmount} and {loanProduct.MaxLoanAmount}.");
        }

        // Validate term against product limits
        if (request.TermMonths < loanProduct.MinTermMonths || request.TermMonths > loanProduct.MaxTermMonths)
        {
            throw new InvalidOperationException(
                $"Term must be between {loanProduct.MinTermMonths} and {loanProduct.MaxTermMonths} months.");
        }

        // Generate unique loan number
        var loanNumber = await GenerateUniqueLoanNumber(cancellationToken).ConfigureAwait(false);

        var loan = Loan.Create(
            member.Id,
            loanProduct.Id,
            loanNumber,
            request.RequestedAmount,
            loanProduct.InterestRate,
            request.TermMonths,
            request.RepaymentFrequency,
            request.Purpose
        );

        await repository.AddAsync(loan, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan {LoanNumber} created for member {MemberId}", loan.LoanNumber, loan.MemberId);

        return new CreateLoanResponse(loan.Id, loan.LoanNumber, loan.Status);
    }

    private async Task<string> GenerateUniqueLoanNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"LN{date:yyyyMMdd}";
        var sequence = 1;

        while (true)
        {
            var loanNumber = $"{prefix}{sequence:D4}";
            var exists = await repository.AnyAsync(
                new LoanByLoanNumberSpec(loanNumber), cancellationToken).ConfigureAwait(false);

            if (!exists)
            {
                return loanNumber;
            }

            sequence++;
        }
    }
}
