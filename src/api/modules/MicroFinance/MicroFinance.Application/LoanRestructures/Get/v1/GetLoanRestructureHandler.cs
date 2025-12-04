using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Get.v1;

public sealed class GetLoanRestructureHandler(
    [FromKeyedServices("microfinance:loanrestructures")] IReadRepository<LoanRestructure> repository)
    : IRequestHandler<GetLoanRestructureRequest, LoanRestructureResponse>
{
    public async Task<LoanRestructureResponse> Handle(
        GetLoanRestructureRequest request,
        CancellationToken cancellationToken)
    {
        var restructure = await repository.FirstOrDefaultAsync(
            new LoanRestructureByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan restructure {request.Id} not found");

        return new LoanRestructureResponse(
            restructure.Id,
            restructure.LoanId,
            restructure.RestructureNumber,
            restructure.RestructureType,
            restructure.Reason,
            restructure.RequestDate,
            restructure.EffectiveDate,
            restructure.OriginalPrincipal,
            restructure.OriginalInterestRate,
            restructure.OriginalRemainingTerm,
            restructure.OriginalInstallmentAmount,
            restructure.NewPrincipal,
            restructure.NewInterestRate,
            restructure.NewTerm,
            restructure.NewInstallmentAmount,
            restructure.GracePeriodMonths,
            restructure.WaivedAmount,
            restructure.RestructureFee,
            restructure.Status,
            restructure.ApprovedByUserId,
            restructure.ApprovedBy,
            restructure.ApprovedAt);
    }
}
