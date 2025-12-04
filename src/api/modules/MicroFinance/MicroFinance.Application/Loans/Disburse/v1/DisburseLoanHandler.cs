using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Disburse.v1;

public sealed class DisburseLoanHandler(
    [FromKeyedServices("microfinance:loans")] IRepository<Loan> repository,
    [FromKeyedServices("microfinance:loanproducts")] IRepository<LoanProduct> loanProductRepository,
    [FromKeyedServices("microfinance:loanschedules")] IRepository<LoanSchedule> scheduleRepository,
    ILogger<DisburseLoanHandler> logger)
    : IRequestHandler<DisburseLoanCommand, DisburseLoanResponse>
{
    public async Task<DisburseLoanResponse> Handle(DisburseLoanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loan = await repository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.Id} not found.");
        }

        if (loan.Status != Loan.StatusApproved)
        {
            throw new InvalidOperationException($"Only approved loans can be disbursed. Current status: {loan.Status}");
        }

        // Get the loan product for interest calculation method
        var loanProduct = await loanProductRepository.FirstOrDefaultAsync(
            new LoanProductByIdSpec(loan.LoanProductId), cancellationToken).ConfigureAwait(false);

        if (loanProduct is null)
        {
            throw new NotFoundException($"Loan product not found for loan {loan.LoanNumber}.");
        }

        var disbursementDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var expectedEndDate = CalculateExpectedEndDate(disbursementDate, loan.TermMonths, loan.RepaymentFrequency);

        loan.Disburse(disbursementDate, expectedEndDate);

        // Generate repayment schedule
        var schedules = GenerateRepaymentSchedule(
            loan,
            loanProduct.InterestMethod,
            disbursementDate
        );

        foreach (var schedule in schedules)
        {
            await scheduleRepository.AddAsync(schedule, cancellationToken).ConfigureAwait(false);
        }

        await repository.UpdateAsync(loan, cancellationToken).ConfigureAwait(false);
        await scheduleRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Loan {LoanNumber} disbursed. Amount: {Amount}, Method: {Method}",
            loan.LoanNumber,
            loan.PrincipalAmount,
            request.DisbursementMethod);

        return new DisburseLoanResponse(
            loan.Id,
            loan.Status,
            disbursementDate,
            expectedEndDate,
            schedules.Count
        );
    }

    private static DateOnly CalculateExpectedEndDate(DateOnly startDate, int termMonths, string frequency)
    {
        return frequency switch
        {
            "DAILY" => startDate.AddDays(termMonths * 30),
            "WEEKLY" => startDate.AddDays(termMonths * 4 * 7),
            "BIWEEKLY" => startDate.AddDays(termMonths * 2 * 14),
            "MONTHLY" => startDate.AddMonths(termMonths),
            _ => startDate.AddMonths(termMonths)
        };
    }

    private static List<LoanSchedule> GenerateRepaymentSchedule(
        Loan loan,
        string interestMethod,
        DateOnly disbursementDate)
    {
        var schedules = new List<LoanSchedule>();
        var numberOfInstallments = CalculateNumberOfInstallments(loan.TermMonths, loan.RepaymentFrequency);

        if (interestMethod.Equals("FLAT", StringComparison.OrdinalIgnoreCase))
        {
            // Flat rate: Total interest is calculated upfront on principal
            var totalInterest = loan.PrincipalAmount * (loan.InterestRate / 100) * (loan.TermMonths / 12.0m);
            var principalPerInstallment = loan.PrincipalAmount / numberOfInstallments;
            var interestPerInstallment = totalInterest / numberOfInstallments;

            for (int i = 1; i <= numberOfInstallments; i++)
            {
                var dueDate = CalculateDueDate(disbursementDate, i, loan.RepaymentFrequency);
                var schedule = LoanSchedule.Create(
                    loan.Id,
                    i,
                    dueDate,
                    principalPerInstallment,
                    interestPerInstallment
                );
                schedules.Add(schedule);
            }
        }
        else
        {
            // Declining/Reducing balance: Interest calculated on outstanding balance
            var outstandingPrincipal = loan.PrincipalAmount;
            var principalPerInstallment = loan.PrincipalAmount / numberOfInstallments;
            var monthlyRate = loan.InterestRate / 100 / 12;

            for (int i = 1; i <= numberOfInstallments; i++)
            {
                var interestForPeriod = outstandingPrincipal * monthlyRate;
                var dueDate = CalculateDueDate(disbursementDate, i, loan.RepaymentFrequency);

                var schedule = LoanSchedule.Create(
                    loan.Id,
                    i,
                    dueDate,
                    principalPerInstallment,
                    interestForPeriod
                );
                schedules.Add(schedule);

                outstandingPrincipal -= principalPerInstallment;
            }
        }

        return schedules;
    }

    private static int CalculateNumberOfInstallments(int termMonths, string frequency)
    {
        return frequency switch
        {
            "DAILY" => termMonths * 30,
            "WEEKLY" => termMonths * 4,
            "BIWEEKLY" => termMonths * 2,
            "MONTHLY" => termMonths,
            _ => termMonths
        };
    }

    private static DateOnly CalculateDueDate(DateOnly startDate, int installmentNumber, string frequency)
    {
        return frequency switch
        {
            "DAILY" => startDate.AddDays(installmentNumber),
            "WEEKLY" => startDate.AddDays(installmentNumber * 7),
            "BIWEEKLY" => startDate.AddDays(installmentNumber * 14),
            "MONTHLY" => startDate.AddMonths(installmentNumber),
            _ => startDate.AddMonths(installmentNumber)
        };
    }
}
