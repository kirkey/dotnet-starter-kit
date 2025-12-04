using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.UpdateAmount.v1;

/// <summary>
/// Handler for updating guaranteed amount.
/// </summary>
public sealed class UpdateGuaranteedAmountHandler(
    IRepository<LoanGuarantor> repository,
    ILogger<UpdateGuaranteedAmountHandler> logger)
    : IRequestHandler<UpdateGuaranteedAmountCommand, UpdateGuaranteedAmountResponse>
{
    public async Task<UpdateGuaranteedAmountResponse> Handle(UpdateGuaranteedAmountCommand request, CancellationToken cancellationToken)
    {
        var guarantor = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Loan guarantor with ID {request.Id} not found.");

        guarantor.UpdateGuaranteedAmount(request.GuaranteedAmount);

        await repository.UpdateAsync(guarantor, cancellationToken);
        logger.LogInformation("Updated guaranteed amount for guarantor {GuarantorId} to {Amount}", request.Id, request.GuaranteedAmount);

        return new UpdateGuaranteedAmountResponse(guarantor.Id, guarantor.GuaranteedAmount, "Guaranteed amount updated successfully.");
    }
}
