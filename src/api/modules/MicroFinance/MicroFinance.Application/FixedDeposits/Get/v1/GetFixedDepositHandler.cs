using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;

public sealed class GetFixedDepositHandler(
    [FromKeyedServices("microfinance:fixeddeposits")] IRepository<FixedDeposit> repository)
    : IRequestHandler<GetFixedDepositRequest, FixedDepositResponse>
{
    public async Task<FixedDepositResponse> Handle(GetFixedDepositRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fixedDeposit = await repository.FirstOrDefaultAsync(
            new FixedDepositByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (fixedDeposit is null)
        {
            throw new NotFoundException($"Fixed deposit with ID {request.Id} not found.");
        }

        return new FixedDepositResponse(
            fixedDeposit.Id,
            fixedDeposit.CertificateNumber,
            fixedDeposit.MemberId,
            fixedDeposit.SavingsProductId,
            fixedDeposit.LinkedSavingsAccountId,
            fixedDeposit.PrincipalAmount,
            fixedDeposit.InterestRate,
            fixedDeposit.TermMonths,
            fixedDeposit.DepositDate,
            fixedDeposit.MaturityDate,
            fixedDeposit.InterestEarned,
            fixedDeposit.InterestPaid,
            fixedDeposit.MaturityInstruction,
            fixedDeposit.Status,
            fixedDeposit.ClosedDate,
            fixedDeposit.Notes
        );
    }
}
