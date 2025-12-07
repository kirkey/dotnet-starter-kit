using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Create.v1;

/// <summary>
/// Handler for creating fee charge.
/// </summary>
public sealed class CreateFeeChargeHandler(
    [FromKeyedServices("microfinance:feecharges")] IRepository<FeeCharge> repository,
    ILogger<CreateFeeChargeHandler> logger)
    : IRequestHandler<CreateFeeChargeCommand, CreateFeeChargeResponse>
{
    public async Task<CreateFeeChargeResponse> Handle(CreateFeeChargeCommand request, CancellationToken cancellationToken)
    {
        var feeCharge = FeeCharge.Create(
            request.FeeDefinitionId,
            request.MemberId,
            request.Reference,
            request.Amount,
            request.LoanId,
            request.SavingsAccountId,
            request.ShareAccountId,
            request.ChargeDate,
            request.DueDate,
            request.Notes);

        await repository.AddAsync(feeCharge, cancellationToken);
        logger.LogInformation("Created fee charge {FeeChargeId} for member {MemberId}", feeCharge.Id, request.MemberId);

        return new CreateFeeChargeResponse(feeCharge.Id, feeCharge.Reference, feeCharge.Amount);
    }
}
