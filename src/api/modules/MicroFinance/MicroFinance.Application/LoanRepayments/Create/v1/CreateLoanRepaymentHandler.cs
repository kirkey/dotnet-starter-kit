using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;

public sealed class CreateLoanRepaymentHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> loanRepository,
    [FromKeyedServices("microfinance:loanrepayments")] IRepository<LoanRepayment> repaymentRepository,
    [FromKeyedServices("microfinance:loanschedules")] IRepository<LoanSchedule> scheduleRepository,
    ILogger<CreateLoanRepaymentHandler> logger)
    : IRequestHandler<CreateLoanRepaymentCommand, CreateLoanRepaymentResponse>
{
    public async Task<CreateLoanRepaymentResponse> Handle(CreateLoanRepaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loan = await loanRepository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.LoanId), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.LoanId} not found.");
        }

        if (loan.Status != Loan.StatusDisbursed)
        {
            throw new InvalidOperationException($"Repayments can only be made on disbursed loans. Current status: {loan.Status}");
        }

        var totalOutstanding = loan.OutstandingPrincipal + loan.OutstandingInterest;
        if (totalOutstanding <= 0)
        {
            throw new InvalidOperationException("Loan has no outstanding balance.");
        }

        var repaymentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var remainingAmount = request.Amount;
        decimal penaltyPaid = 0;
        decimal interestPaid = 0;
        decimal principalPaid = 0;

        // Calculate penalty for overdue installments
        var overdueSchedules = loan.LoanSchedules
            .Where(s => !s.IsPaid && s.DueDate < repaymentDate)
            .OrderBy(s => s.DueDate)
            .ToList();

        // Get loan product for penalty rate
        decimal penaltyRate = 0;
        if (overdueSchedules.Count > 0)
        {
            // Apply late payment penalty (simplified calculation)
            var totalOverdue = overdueSchedules.Sum(s => s.TotalAmount - s.PaidAmount);
            penaltyPaid = Math.Min(remainingAmount, totalOverdue * penaltyRate / 100);
            remainingAmount -= penaltyPaid;
        }

        // Apply payment to interest first
        interestPaid = Math.Min(remainingAmount, loan.OutstandingInterest);
        remainingAmount -= interestPaid;

        // Apply remaining to principal
        principalPaid = Math.Min(remainingAmount, loan.OutstandingPrincipal);

        // Generate receipt number
        var receiptNumber = $"RCP{DateTime.UtcNow:yyyyMMddHHmmss}";

        // Create repayment record using correct method signature
        var repayment = LoanRepayment.Create(
            loan.Id,
            receiptNumber,
            principalPaid,
            interestPaid,
            request.PaymentMethod,
            penaltyPaid,
            repaymentDate,
            request.TransactionReference
        );

        await repaymentRepository.AddAsync(repayment, cancellationToken).ConfigureAwait(false);

        // Update loan outstanding balances
        loan.ApplyRepayment(principalPaid, interestPaid);

        // Update schedule paid amounts
        var unpaidSchedules = loan.LoanSchedules
            .Where(s => !s.IsPaid)
            .OrderBy(s => s.DueDate)
            .ToList();

        var paymentToApply = request.Amount;
        foreach (var schedule in unpaidSchedules)
        {
            if (paymentToApply <= 0) break;

            var amountNeeded = schedule.TotalAmount - schedule.PaidAmount;
            var amountApplied = Math.Min(paymentToApply, amountNeeded);

            schedule.ApplyPayment(amountApplied, repaymentDate);
            await scheduleRepository.UpdateAsync(schedule, cancellationToken).ConfigureAwait(false);

            paymentToApply -= amountApplied;
        }

        await loanRepository.UpdateAsync(loan, cancellationToken).ConfigureAwait(false);
        await repaymentRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await scheduleRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await loanRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var remainingBalance = loan.OutstandingPrincipal + loan.OutstandingInterest;

        logger.LogInformation(
            "Payment of {Amount} applied to loan {LoanNumber}. Principal: {Principal}, Interest: {Interest}, Penalty: {Penalty}",
            request.Amount,
            loan.LoanNumber,
            principalPaid,
            interestPaid,
            penaltyPaid);

        return new CreateLoanRepaymentResponse(
            repayment.Id,
            principalPaid,
            interestPaid,
            penaltyPaid,
            remainingBalance
        );
    }
}
