using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;

/// <summary>
/// Handler for retrieving a loan repayment by ID.
/// </summary>
public sealed class GetLoanRepaymentHandler(
    ILogger<GetLoanRepaymentHandler> logger,
    [FromKeyedServices("microfinance:loanrepayments")] IReadRepository<LoanRepayment> repository,
    ICacheService cacheService)
    : IRequestHandler<GetLoanRepaymentRequest, LoanRepaymentResponse>
{
    public async Task<LoanRepaymentResponse> Handle(GetLoanRepaymentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cacheKey = $"loanrepayment:{request.Id}";

        var cached = await cacheService.GetAsync<LoanRepaymentResponse>(cacheKey, cancellationToken).ConfigureAwait(false);
        if (cached is not null)
        {
            return cached;
        }

        var repayment = await repository.FirstOrDefaultAsync(
            new LoanRepaymentByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (repayment is null)
        {
            throw new NotFoundException($"Loan repayment with ID {request.Id} not found.");
        }

        var response = new LoanRepaymentResponse(
            repayment.Id,
            repayment.LoanId,
            repayment.Loan?.LoanNumber,
            repayment.Loan?.Member != null ? $"{repayment.Loan.Member.FirstName} {repayment.Loan.Member.LastName}" : null,
            repayment.ReceiptNumber,
            repayment.RepaymentDate,
            repayment.PrincipalAmount,
            repayment.InterestAmount,
            repayment.PenaltyAmount,
            repayment.TotalAmount,
            repayment.PaymentMethod,
            repayment.Notes);

        await cacheService.SetAsync(cacheKey, response, cancellationToken: cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved loan repayment {LoanRepaymentId}", request.Id);
        return response;
    }
}
