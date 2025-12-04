using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Approve.v1;

/// <summary>
/// Handler for approving a guarantor.
/// </summary>
public sealed class ApproveGuarantorHandler(
    IRepository<LoanGuarantor> repository,
    ILogger<ApproveGuarantorHandler> logger)
    : IRequestHandler<ApproveGuarantorCommand, ApproveGuarantorResponse>
{
    public async Task<ApproveGuarantorResponse> Handle(ApproveGuarantorCommand request, CancellationToken cancellationToken)
    {
        var guarantor = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Loan guarantor with ID {request.Id} not found.");

        guarantor.Approve();

        await repository.UpdateAsync(guarantor, cancellationToken);
        logger.LogInformation("Approved guarantor {GuarantorId}", request.Id);

        return new ApproveGuarantorResponse(guarantor.Id, guarantor.Status, "Guarantor approved successfully.");
    }
}
