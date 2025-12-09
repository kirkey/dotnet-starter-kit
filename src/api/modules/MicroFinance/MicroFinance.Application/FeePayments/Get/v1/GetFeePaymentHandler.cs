using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Get.v1;

/// <summary>
/// Handler for getting a fee payment by ID.
/// </summary>
public sealed class GetFeePaymentHandler(
    [FromKeyedServices("microfinance:feepayments")] IReadRepository<FeePayment> repository,
    ILogger<GetFeePaymentHandler> logger)
    : IRequestHandler<GetFeePaymentRequest, FeePaymentResponse>
{
    public async Task<FeePaymentResponse> Handle(GetFeePaymentRequest request, CancellationToken cancellationToken)
    {
        var spec = new FeePaymentByIdSpec(request.Id);
        var feePayment = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (feePayment is null)
        {
            throw new NotFoundException($"Fee payment with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved fee payment {FeePaymentId}", request.Id);

        return feePayment;
    }
}
