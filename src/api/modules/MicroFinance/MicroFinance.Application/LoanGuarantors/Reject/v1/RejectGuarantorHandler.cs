using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Reject.v1;

/// <summary>
/// Handler for rejecting a guarantor.
/// </summary>
public sealed class RejectGuarantorHandler(
    [FromKeyedServices("microfinance:loanguarantors")] IRepository<LoanGuarantor> repository,
    ILogger<RejectGuarantorHandler> logger)
    : IRequestHandler<RejectGuarantorCommand, RejectGuarantorResponse>
{
    public async Task<RejectGuarantorResponse> Handle(RejectGuarantorCommand request, CancellationToken cancellationToken)
    {
        var guarantor = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Loan guarantor with ID {request.Id} not found.");

        guarantor.Reject(request.Reason);

        await repository.UpdateAsync(guarantor, cancellationToken);
        logger.LogInformation("Rejected guarantor {GuarantorId}", request.Id);

        return new RejectGuarantorResponse(guarantor.Id, guarantor.Status, "Guarantor rejected successfully.");
    }
}
