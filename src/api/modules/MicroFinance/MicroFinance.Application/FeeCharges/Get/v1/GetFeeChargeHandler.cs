using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;

public sealed class GetFeeChargeHandler(
    [FromKeyedServices("microfinance:feecharges")] IRepository<FeeCharge> repository)
    : IRequestHandler<GetFeeChargeRequest, FeeChargeResponse>
{
    public async Task<FeeChargeResponse> Handle(GetFeeChargeRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var feeCharge = await repository.FirstOrDefaultAsync(
            new FeeChargeByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (feeCharge is null)
        {
            throw new NotFoundException($"Fee charge with ID {request.Id} not found.");
        }

        return new FeeChargeResponse(
            feeCharge.Id,
            feeCharge.FeeDefinitionId,
            feeCharge.MemberId,
            feeCharge.LoanId,
            feeCharge.SavingsAccountId,
            feeCharge.ShareAccountId,
            feeCharge.Reference,
            feeCharge.ChargeDate,
            feeCharge.DueDate,
            feeCharge.Amount,
            feeCharge.AmountPaid,
            feeCharge.Outstanding,
            feeCharge.Status,
            feeCharge.PaidDate,
            feeCharge.Notes
        );
    }
}
